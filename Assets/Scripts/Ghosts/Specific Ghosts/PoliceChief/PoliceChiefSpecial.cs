using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PoliceChiefSpecial : MonoBehaviour, ISpecialMove, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

    private bool shouldChangeBack = true;
    private PlayerStateMachine playerStateMachine;
    private Animator camAnim;
    private Camera cam;

    [HideInInspector] public PoliceChiefManager manager;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        if (manager != null)
        {
            if (manager.getSpecialCooldown() > 0)
            {
                playerStateMachine.OnCooldown("c_special");
            }
            else
            {
                playerStateMachine.OffCooldown("c_special");
            }
        }
    }

    void CheckPullBack()
    {
        if (shouldChangeBack) {
            camAnim.SetBool("pullBack", false);
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        shouldChangeBack = true;
    }

    void StartSpecialChargeUp()
    {
        camAnim.SetBool("pullBack", true);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    void StopSpecialChargeUp()
    {
        CheckPullBack();
    }

    void StartSpecialPrimed()
    {
        shouldChangeBack = false;
    }

    void StopSpecialPrimed()
    {
        CheckPullBack();
    }

    void StartSpecialAttack()
    {
        StartCoroutine(StartSpecialAttackCoroutine());
    }

    private IEnumerator StartSpecialAttackCoroutine()
    {
        shouldChangeBack = false;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 dir = (mousePos - pos).normalized;
        //GameObject enemyHit = null;

        for (int i = 0; i < manager.GetStats().ComputeValue("Special Ricochet Count") + 1; i++)
        {
            if (i > 0) yield return new WaitForSeconds(0.08f);
            CameraShake.instance.Shake(0.35f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
            RaycastHit2D hit = Physics2D.Raycast(pos, dir, manager.GetStats().ComputeValue("Special Travel Distance"), LayerMask.GetMask("Ground"));
            RaycastHit2D[] enemyHits = Physics2D.RaycastAll(pos, (hit.point - pos), Vector2.Distance(pos, hit.point), LayerMask.GetMask("Enemy"));

            Debug.DrawLine(pos, hit.point, Color.red, 5.0f);
            GameObject railgunTracer = Instantiate(manager.specialRailgunTracer, Vector3.zero, Quaternion.identity);
            LineRenderer lineRenderer = railgunTracer.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, pos);
            lineRenderer.SetPosition(1, hit.point);
            foreach(RaycastHit2D enemyHit in enemyHits)
            {
                enemyHit.transform.gameObject.GetComponent<Health>().Damage(manager.specialDamage, gameObject);
            }

            float hitAngle = Vector2.Angle((pos - hit.point), hit.normal);
            Debug.Log("NORTH RAILGUN HIT ANGLE: " + hitAngle);
            if (hitAngle < manager.GetStats().ComputeValue("Special Ricochet Minimum Normal Angle")) break;
            Vector2 reflect = Vector2.Reflect(dir, hit.normal);
            Debug.Log("This is normal" + hit.normal);
            pos = hit.point + new Vector2(reflect.x * 0.1f, reflect.y * 0.1f);
            dir = reflect.normalized;
        }
    }

    void StopSpecialAttack()
    {
        playerStateMachine.OnCooldown("c_special");
        camAnim.SetBool("pullBack", false);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        manager.startSpecialCooldown();
    }

    public bool GetBool()
    {
        return true;
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
