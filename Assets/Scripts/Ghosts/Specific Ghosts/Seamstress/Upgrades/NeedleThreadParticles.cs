using UnityEngine;

public class NeedleThreadParticles : MonoBehaviour
{
    [SerializeField] float baseParticleVelocity;
    [SerializeField] float baseParticleAccel;

    [Header("How much deviation the particles will experience")]
    [SerializeField] float deviationMagnitude;
    private Vector2[] deviationStrength; // how much each particle deviate from a straight path

    [Header("How quickly the particles will correct its deviation")]
    [SerializeField] float correctionMagnitude;

    private Collider2D[] targets;
    private bool initiated;
    private ParticleSystem system;
    private ParticleSystem.Particle[] particles;
    private int particleCount;
    private bool[] collidedParticles; // simply used to keep track of which particles has already collided with enemies,
                                      // deleting them is messy so Im just gonna hide them
    private int debuffStrength;
    private float debuffDuration;

    [SerializeField] ParticleSystemForceField forceField;
    [SerializeField] float lockOnTime = 1;

    [SerializeField] private GameObject debuffVFX;

    [SerializeField] private DamageContext needleDamage;
    [SerializeField] private float damage;

    public void Init(Collider2D[] targets, int debuffStrength, float debuffDuration)
    {
        this.targets = targets;
        this.debuffStrength = debuffStrength;
        this.debuffDuration = debuffDuration;
        initiated = true;
    }

    void Start()
    {
        system = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[targets.Length];
        collidedParticles = new bool[targets.Length];
        deviationStrength = new Vector2[targets.Length];

        needleDamage.damage = damage;

        EmitNeedles(targets);
    }

    void LateUpdate()
    {
        if (initiated && lockOnTime < 0f)
        {
            forceField.enabled = false;
            UpdateNeedleVelocity(Time.deltaTime);
        }
        else lockOnTime -= Time.deltaTime;
    }

    public void EmitNeedles(Collider2D[] enemies)
    {
        ParticleSystem.EmitParams emitParams = new();
        emitParams.startLifetime = 4f;

        // emit the particles
        system.Emit(emitParams, enemies.Length);
        particleCount = system.GetParticles(particles);

        // initialize the deviation and velocity of each particle
        for (int i = 0; i < particleCount; i++)
        {
            Vector2 dir = (enemies[i].transform.position - particles[i].position).normalized;
            Vector2 normal = new Vector2[2] { new Vector2(-dir.y, dir.x), new Vector2(dir.y, -dir.x) }[Random.Range(0, 2)]; // randomly select a normal vector in either direction
            deviationStrength[i] = normal * deviationMagnitude;
            particles[i].velocity = baseParticleVelocity * dir + deviationStrength[i];
        }
    }

    private void UpdateNeedleVelocity(float delta)
    {
        particleCount = system.GetParticles(particles);
        
        // if particles have already expired, then destroy before SetParticle is called again
        if (particleCount == 0)
        {
            // failure to do so can lead to particles permanently persisting on screen
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < particleCount; i++)
        {
            if (collidedParticles[i]) continue; // if this particle has already "collided" with an enemy, do not do anything with it

            if (targets[i] == null)
            {
                particles[i].startColor = new Color(0, 0, 0, 0); // hide that particle after "collision"
                continue;
            }

            // if particle "collides" with its target enemy
            Vector2 dist = targets[i].transform.position - particles[i].position;
            if (dist.sqrMagnitude <= 0.1f) 
            {
                particles[i].startColor = new Color(0, 0, 0, 0); // hide that particle after "collision"
                collidedParticles[i] = true;
                targets[i].GetComponent<Health>().Damage(needleDamage, PlayerID.instance.gameObject);
                if (!targets[i].gameObject.GetComponent<NeedleAndThreadDebuff>()) // GetComponent check should only run once thanks to collidedParticles[]
                {
                    NeedleAndThreadDebuff debuff = targets[i].gameObject.AddComponent<NeedleAndThreadDebuff>();
                    debuff.Init(debuffStrength, debuffDuration); // TODO hook up to NeedleAndThread skill
                    GameObject needleDebuffVFX = Instantiate(debuffVFX, targets[i].gameObject.transform);
                    Destroy(needleDebuffVFX, debuffDuration);
                }
                continue;
            }

            // reduce the deviation
            deviationStrength[i] = new Vector2(Mathf.Max(Mathf.Abs(deviationStrength[i].x) - correctionMagnitude * delta, 0) * Mathf.Sign(deviationStrength[i].x),
                                               Mathf.Max(Mathf.Abs(deviationStrength[i].y) - correctionMagnitude * delta, 0) * Mathf.Sign(deviationStrength[i].y));

            Debug.DrawLine(particles[i].position, particles[i].position + particles[i].velocity, Color.white);
            Debug.DrawLine(particles[i].position, (Vector2)particles[i].position + 2 * dist.normalized, Color.blue);
            Debug.DrawLine(particles[i].position, (Vector2)particles[i].position + deviationStrength[i], Color.red);

            // handle particle arc
            Vector2 dir = dist.normalized;
            baseParticleVelocity += baseParticleAccel * delta;
            particles[i].velocity = baseParticleVelocity * dir + deviationStrength[i];
        }
        system.SetParticles(particles, particles.Length);
    }
}
