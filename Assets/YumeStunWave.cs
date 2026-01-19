using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class YumeStunWave : MonoBehaviour
{
    [SerializeField] private float knockbackStrength;
    [SerializeField] private float knockupStrength;
    [SerializeField] private float climbUpHeight;
    [SerializeField] private float dropDownHeight;
    [SerializeField] private DamageContext waveDamage;
    [SerializeField] private ParticleSystem particles;

    private float speed;
    private float duration;
    private float stunTime;

    private List<GameObject> hitEnemies;
    private float durationTimer;
    [HideInInspector] public bool isWaving = false;


    // Start is called before the first frame update
    void Awake()
    {
        hitEnemies = new List<GameObject>();
        isWaving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaving && durationTimer > 0f)
        {
            durationTimer -= Time.deltaTime;
            if (durationTimer <= 0f)
            {
                EndWave();
            }
        }

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (!isWaving) return;

        float direction = Mathf.Sign(gameObject.transform.rotation.y);

        // Forward Movement
        transform.position += ((Vector3.right * direction) * (speed * Time.deltaTime));

        // Climb Up
        Collider2D[] overlaps = Physics2D.OverlapCircleAll((transform.position + new Vector3(0.3f * direction, 0.5f)), 0.2f, LayerMask.GetMask("Ground"));
        if (overlaps.Length > 0)
        {
            Collider2D[] upOverlaps = Physics2D.OverlapCircleAll((transform.position + new Vector3(0.3f * direction, climbUpHeight + 0.3f)), 0.2f, LayerMask.GetMask("Ground"));
            if (upOverlaps.Length > 0)
            {
                EndWave();
                return;
            }

            RaycastHit2D upHit = Physics2D.Raycast((transform.position + new Vector3(0.3f * direction, climbUpHeight)), Vector2.down, dropDownHeight, LayerMask.GetMask("Ground"));
            if (upHit)
            {
                //transform.position = new Vector3(transform.position.x, upHit.point.y, transform.position.z);
                transform.position = upHit.point;
            }
            else
            {
                EndWave();
                return;
            }
        }

        // Drop Down
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, dropDownHeight, LayerMask.GetMask("Ground"));
        if (hit)
        {
            transform.position = hit.point;
        }
        else
        {
            RaycastHit2D farHit = Physics2D.Raycast(transform.position + new Vector3(0.3f * direction, 0f), Vector2.down, dropDownHeight, LayerMask.GetMask("Ground"));
            if (farHit)
            {
                transform.position = farHit.point;
            }
            else
            {
                EndWave();
                return;
            }
        }
    }

    public void StartWave(float speed, float duration, float stunTime, float damage, List<GameObject> enemiesHit)
    {
        AudioManager.Instance.SFXBranch.GetSFXTrack("Yume-Fatebound Damage").SetPitch(1f, 1f);
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Yume-Fatebound Damage");
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Yume-Stun Damage");

        this.speed = speed;
        this.duration = duration;
        this.stunTime = stunTime;
        this.waveDamage.damage = damage;

        particles.gameObject.transform.position += new Vector3(0f, 0f, 3f);

        hitEnemies = new List<GameObject>();
        foreach (GameObject enemy in enemiesHit)
        {
            HitEnemy(enemy, false);
        }

        durationTimer = duration;
        isWaving = true;
    }

    public void EndWave()
    {
        //Destroy(gameObject);
        StartCoroutine(EndWaveCoroutine());
    }

    private IEnumerator EndWaveCoroutine()
    {
        isWaving = false;
        GetComponent<BoxCollider2D>().enabled = false;
        ParticleSystem.EmissionModule emissionModule = particles.emission;
        emissionModule.rateOverTime = 0f;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isWaving) return;
        if (collision.gameObject.CompareTag("Enemy") && !hitEnemies.Contains(collision.gameObject))
        {
            GameObject enemy = collision.gameObject;
            HitEnemy(enemy, true);
            /*
            hitEnemies.Add(enemy);
            enemy.GetComponent<Health>().Damage(waveDamage, PlayerID.instance.gameObject);
            if (enemy.GetComponent<EnemyStateManager>() != null)
            {
                enemy.GetComponent<EnemyStateManager>().Stun(waveDamage, stunTime);
                enemy.GetComponent<EnemyStateManager>().ApplyKnockback(Vector3.up, knockupStrength, 0.3f);
                enemy.GetComponent<EnemyStateManager>().ApplyKnockback(enemy.transform.position - gameObject.transform.position, knockbackStrength, 0.3f);
            }
            */
        }
    }



    private void HitEnemy(GameObject enemy, bool applyKnockback)
    {
        if (hitEnemies.Contains(enemy)) return;

        AudioManager.Instance.SFXBranch.PlaySFXTrack("Yume-Stun Damage");
        AudioManager.Instance.VABranch.PlayVATrack("Yume-Seamstress Heavy Stun");

        hitEnemies.Add(enemy);
        float damageDealt = enemy.GetComponent<Health>().Damage(waveDamage, PlayerID.instance.gameObject);
        if (enemy.GetComponent<EnemyStateManager>() != null && damageDealt > 0f)
        {
            enemy.GetComponent<EnemyStateManager>().Stun(waveDamage, stunTime);
            if (applyKnockback)
            {
                enemy.GetComponent<EnemyStateManager>().ApplyKnockback(Vector3.up, knockupStrength, 0.3f);
                enemy.GetComponent<EnemyStateManager>().ApplyKnockback(enemy.transform.position - gameObject.transform.position, knockbackStrength, 0.3f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isWaving) return;
    }
}
