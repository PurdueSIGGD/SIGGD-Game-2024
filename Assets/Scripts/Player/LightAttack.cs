using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This is the script that contains the code form the player's melee attack
/// </summary>
public class LightAttack : MonoBehaviour
{
    [SerializeField] float range = 1.2f; // radius of the attack cone
    [SerializeField] float angle = 80f; // angle of the attack cone
    [SerializeField] DamageContext lightDamage;
    [SerializeField] float cooldown = 1; // Cooldown of player attack
    [SerializeField] float rayCount = 6; // number of rays used to check for collision
    [SerializeField] LayerMask attackMask;

    private HashSet<int> hits; // stores which targets have already been hit in one attack
    private Camera mainCamera;
    
    private void Start()
    {
        hits = new HashSet<int>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    /// <summary>
    /// Damage enemies in a cone shape in the direction of the cursor
    /// </summary>
    public void StartLightAttack()
    {
        float halfAngle = angle / 2; // angle above and below the centerline of the attack cone
        float deltaAngle = halfAngle / rayCount * 2; // change in degree between each ray

        Vector2 orig = transform.position;
        Vector2 center = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position); // center ray
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

    /// <summary>
    /// Cast a ray in some deviation in angle from the centerRay
    /// Check for 
    /// </summary>
    private void CastRay(Vector2 orig, float angle, Vector3 centerRay)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        RaycastHit2D hit = Physics2D.Raycast(orig, rot * centerRay, range, attackMask);

#if DEBUG
        Debug.DrawLine(orig, rot * centerRay * range + transform.position, Color.blue, 1.0f);
#endif

        // check if hit is valid
        if (hit.collider == null || !hits.Add(hit.collider.gameObject.GetInstanceID()))
        {
            return;
        }
        IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(lightDamage, gameObject);
        }
    }
}
