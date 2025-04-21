using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// This is the script that contains the code form the player's melee attack
/// </summary>
public class LightAttack : MonoBehaviour, IStatList
{
    [SerializeField] float range = 1.2f; // radius of the attack cone
    [SerializeField] float angle = 80f; // angle of the attack cone
    [SerializeField] DamageContext lightDamage;
    [SerializeField] StatManager.Stat[] statList;
    [SerializeField] float cooldown = 1; // Cooldown of player attack
    [SerializeField] float rayCount = 6; // number of rays used to check for collision
    [SerializeField] LayerMask attackMask;

    private HashSet<int> hits; // stores which targets have already been hit in one attack
    private Camera mainCamera;
    private StatManager stats;
    private Vector2 lastMousePos;

    private void Start()
    {
        hits = new HashSet<int>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        stats = gameObject.GetComponent<StatManager>();
        lightDamage.damage = stats.ComputeValue("Damage");
    }

    /// <summary>
    /// Damage enemies in a cone shape in the direction of the cursor
    /// </summary>
    public void StartLightAttack()
    {
        GetComponent<PlayerStateMachine>().ConsumeLightAttackInput();
        GetComponent<Move>().PlayerStop();

        float halfAngle = angle / 2; // angle above and below the centerline of the attack cone
        float deltaAngle = halfAngle / rayCount * 2; // change in degree between each ray

        Vector2 orig = transform.position;
        Vector2 mouseDiff = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position); // center ray
        float m_angle = Mathf.Atan2(mouseDiff.x, mouseDiff.y) + Mathf.PI;
        Vector2 center = new Vector2(Mathf.Sign(mouseDiff.x), 0);
        
        if (center.x == 1) // update player facing direction
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (center.x == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (m_angle > 3 * Mathf.PI / 4 && m_angle < 5 * Mathf.PI / 4)
        {
            center = new Vector2(0, 1);
        }
        center = center.normalized;

#if DEBUG        
        Debug.DrawLine(orig, center * range + orig, Color.red, 1.0f);
#endif

        for (int i = 1; i <= rayCount / 2; i ++)
        {
            CastRay(orig, deltaAngle * i, center);
            CastRay(orig, deltaAngle * -i, center);
        }
        hits.Clear(); // re-enable damage to all hit enemy
    }

    public void StopLightAttack()
    {
        GetComponent<Move>().PlayerGo();
    }

    public void CompileSkyAttack()
    {
        float halfAngle = angle / 2; // angle above and below the centerline of the attack cone
        float deltaAngle = halfAngle / rayCount * 2; // change in degree between each ray

        Vector2 orig = transform.position;
#if DEBUG        
        Debug.DrawLine(orig, lastMousePos * range + orig, Color.red, 1.0f);
#endif

        for (int i = 1; i <= rayCount / 2; i++)
        {
            CastRay(orig, deltaAngle * i, lastMousePos);
            CastRay(orig, deltaAngle * -i, lastMousePos);
        }
        hits.Clear(); // re-enable damage to all hit enemy
    }

    public void StartSkyLightAttack()
    {
        GetComponent<Move>().PlayerStop();

        Vector2 orig = transform.position;
        Vector2 mouseDiff = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position); // center ray
        float m_angle = Mathf.Atan2(mouseDiff.x, mouseDiff.y) + Mathf.PI;

        Vector2 center = new Vector2(Mathf.Sign(mouseDiff.x), 0);

        if (center.x == 1) // update player facing direction
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (center.x == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        //transform.localScale = new Vector2(center.x, 1);
        if (m_angle > 3 * Mathf.PI / 4 && m_angle < 5 * Mathf.PI / 4)
        {
            center = new Vector2(0, 1);
        }
        if (m_angle < Mathf.PI / 4 || m_angle > 7 * Mathf.PI / 4)
        {
            center = new Vector2(0, -1);
        }
        Debug.Log(m_angle);
        Debug.Log(center);
        center = center.normalized;
        lastMousePos = center;

        Debug.Log(center * 10);
        GetComponent<Rigidbody2D>().AddForce(center * 10, ForceMode2D.Impulse);
    }

    public void StopSkyLightAttack()
    {
        GetComponent<Move>().PlayerGo();
    }

    /// <summary>
    /// Cast a ray in some deviation in angle from the centerRay
    /// Check for 
    /// </summary>
    private void CastRay(Vector2 orig, float angle, Vector3 centerRay)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        RaycastHit2D[] hit = Physics2D.RaycastAll(orig, rot * centerRay, range, attackMask);

#if DEBUG
        Debug.DrawLine(orig, rot * centerRay * range + transform.position, Color.blue, 1.0f);
#endif

        foreach (RaycastHit2D h in hit)
        {
            // check if hit is valid
            if (h.collider == null || !hits.Add(h.collider.gameObject.GetInstanceID()))
            {
                return;
            }

            foreach (IDamageable damageable in h.collider.gameObject.GetComponents<IDamageable>())
            {
                damageable.Damage(lightDamage, gameObject);
            }
        }
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
