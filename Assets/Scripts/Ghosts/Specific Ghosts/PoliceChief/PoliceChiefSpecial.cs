using System.Collections;
using UnityEngine;

public class PoliceChiefSpecial : MonoBehaviour, ISpecialMove, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

    private bool shouldChangeBack = true;
    private PlayerStateMachine playerStateMachine;
    private Animator camAnim;
    private Camera cam;
    private StatManager stats;

    [HideInInspector] public PoliceChiefManager manager;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        stats = GetComponent<StatManager>();
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
        shouldChangeBack = false;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 dir = (mousePos - pos).normalized;
        GameObject enemyHit = null;

        for (int i = 0; i < stats.ComputeValue("Bounces") + 1; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(pos, dir, stats.ComputeValue("Distance"), LayerMask.GetMask("Ground", "Enemy"));

            Debug.DrawLine(pos, hit.point, Color.red, 5.0f);

            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                enemyHit = hit.transform.gameObject;
                break;
            }
            else
            {
                Vector2 reflect = Vector2.Reflect(dir, hit.normal);
                Debug.Log("This is normal" + hit.normal);
                pos = hit.point + new Vector2(reflect.x * 0.1f, reflect.y * 0.1f);
                dir = reflect.normalized;
                //mousePos = pos + reflect.normalized;
            }
        }

        if (enemyHit != null)
        {
            Debug.Log("Hit Something");
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
