using System.Collections;
using UnityEngine;

//<summary>
// Special ability script for player - if a ghost is currently posessing and 
// overrides the special move delegate, that delegate will run. Else, the player
// dash function will run like normal.
// Base player action: player dashes towards the mouse location in a fixed time
//</summary>
[DisallowMultipleComponent]
public class Dash : MonoBehaviour, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

    private Camera mainCamera;
    private Rigidbody2D rb;

    private Vector2 velocity = Vector2.zero;
    private bool isDashing = false;

    private StatManager stats;
    private OrionManager orionManager;
    private PlayerStateMachine psm;

    [Header("Delegate Override Variables")]
    public SpecialAction specialAction;
    public delegate void SpecialAction(); // delegate to contain any ghost overrides

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<StatManager>();
        orionManager = GetComponent<OrionManager>();
        psm = GetComponent<PlayerStateMachine>();
        specialAction = null;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = velocity;
        }


        if (orionManager != null)
        {
            if (orionManager.getSpecialCooldown() > 0f || orionManager.isAirbornePostDash)
            {
                psm.OnCooldown("c_special");
            }
            else
            {
                psm.OffCooldown("c_special");
            }
        }
    }

    //<summary>
    // Function called through the animation state machine when player is meant to "Dash"
    // Calculates the displacement vector between the player and the mouse and starts the dash
    // Can be overriden by any posesssing ghost's start special ability function.
    //</summary>
    public void StartDash()
    {
        GetComponent<Move>().PlayerStop();
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = ((Vector2) mousePos - (Vector2) transform.position).normalized;
        if (GetComponent<Animator>().GetBool("p_grounded"))
        {
            direction = new Vector2(direction.x, Mathf.Max(direction.y, 0f)).normalized;
        }

        if (direction.x > 0) // update player facing direction
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        Vector2 displacement = direction * stats.ComputeValue("Max Dash Distance");
        this.velocity = displacement / stats.ComputeValue("Dash Time");
        StartCoroutine(DashCoroutine());
    }

    public void StopDash()
    {
        GetComponent<Move>().PlayerGo();
        if (GetComponent<Animator>().GetBool("p_grounded")) return;
        GetComponent<Move>().ApplyKnockback(rb.velocity.normalized, rb.velocity.magnitude, true);
        orionManager.isAirbornePostDash = true;
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;

        yield return new WaitForSeconds(stats.ComputeValue("Dash Time") - 0.05f);
        orionManager.setSpecialCooldown(stats.ComputeValue("Dash Cooldown"));
        yield return new WaitForSeconds(0.05f);

        rb.velocity *= stats.ComputeValue("Post Dash Momentum Fraction");
        psm.EnableTrigger("OPT");
        isDashing = false;
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
