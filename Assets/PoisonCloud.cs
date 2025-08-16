using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    [SerializeField] private ParticleSystem cloudParticleSystem;
    [SerializeField] private float baseParticleEmission;
    [SerializeField] private GameObject circleVFX;
    [SerializeField] private float interval;

    private SilasManager manager;
    private float radius;
    private float cloudDuration = 0f;
    private float timer = 999f;

    private float blightDuration;

    private GameObject circleArea;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cloudDuration > 0f)
        {
            cloudDuration -= Time.deltaTime;
            if (cloudDuration <= 0f)
            {
                circleArea.GetComponent<CircleAreaHandler>().playCircleEnd();
                Destroy(gameObject);
            }
        }

        // Blight damage over time interval
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        SpreadBlight();
        timer = interval;
    }



    public void InitializePoisonCloud(SilasManager manager, float radius, float cloudDuration, float blightDuration)
    {
        this.manager = manager;
        this.radius = radius;
        this.cloudDuration = cloudDuration;
        this.blightDuration = blightDuration;
        timer = 0f;

        //VFX
        circleArea = Instantiate(circleVFX, transform.position, Quaternion.identity);
        circleArea.GetComponent<CircleAreaHandler>().playCircleStart(radius, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor, 0.02f);

        ParticleSystem.ShapeModule shape = cloudParticleSystem.shape;
        shape.scale *= radius;
        ParticleSystem.EmissionModule emission = cloudParticleSystem.emission;
        emission.rateOverTime = baseParticleEmission * radius;
    }



    private void SpreadBlight()
    {
        // Affect Enemies
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.gameObject == null) continue;
            if (enemy.GetComponentInChildren<BlightDebuff>() == null)
            {
                GameObject blight = Instantiate(manager.blightDebuff, enemy.transform);
                blight.GetComponent<BlightDebuff>().ApplyDebuff(manager); // TODO: Add overload with duration field
            }
            else
            {
                enemy.GetComponentInChildren<BlightDebuff>().SetDebuffTime(blightDuration);
            }
        }
    }
}
