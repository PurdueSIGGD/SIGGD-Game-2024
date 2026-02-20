using System.Collections;
using UnityEngine;

public class ClericBubbleShieldScript : MonoBehaviour
{

    [SerializeField] private bool isElite;
    [SerializeField] private float eliteDuration;
    //Health health;
    GameObject parentEnemy;

    private GameObject shieldCircle;
    [SerializeField] private GameObject shieldCircleVFX;
    [SerializeField] private Color shieldCircleColor;
    [SerializeField] private Sprite blockIcon;

    [HideInInspector] public bool isEnding = false;

    void Awake()
    {
        GameplayEventHolder.OnDamageFilter.Add(TransferDamageToBubbleDamageFilter);
    }

    void OnDestroy()
    {
        GameplayEventHolder.OnDamageFilter.Remove(TransferDamageToBubbleDamageFilter);
    }

    void Start()
    {
        //health = GetComponent<Health>();
        if (isElite) StartCoroutine(DurationTimer(eliteDuration));
        shieldCircle = Instantiate(shieldCircleVFX, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        shieldCircle.GetComponent<CircleAreaHandler>().playCircleStart(1.4f, shieldCircleColor);
    }

    public void SetParentEnemy(GameObject enemy)
    {
        parentEnemy = enemy;
    }

    public void TransferDamageToBubbleDamageFilter(ref DamageContext damage)
    {
        if (damage.victim != parentEnemy || isEnding)
        {
            return;
        }
        AudioManager.Instance.SFXBranch.PlaySFXTrack("ClericShieldBlock");
        DamageContext transferDamage = CreateTransferDamageContext(damage);
        //health.Damage(transferDamage, transferDamage.attacker);
        damage.damage = 0; // negate damage to parent enemy
        DamageNumberManager.instance.PlayMessage(gameObject, 0f, blockIcon, "Blocked!", shieldCircleColor);
        print("ABSORBED!");
        return;
    }

    private DamageContext CreateTransferDamageContext(DamageContext damage)
    {
        DamageContext newContext = damage;
        newContext.victim = this.gameObject;
        newContext.extraContext += "[transfered from " + damage.victim.name + "]";
        return newContext;
    }



    private IEnumerator DurationTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }



    public void EndBubble()
    {
        StartCoroutine(EndBubbleCoroutine());
    }

    private IEnumerator EndBubbleCoroutine()
    {
        isEnding = true;
        if (shieldCircle != null) shieldCircle.GetComponent<CircleAreaHandler>().playCircleEnd();
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
