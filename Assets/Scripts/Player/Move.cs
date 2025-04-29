using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

    private InputAction moveInput;
    private InputAction playerActionDown;
    private Rigidbody2D rb;

    private StatManager stats;
    private Animator animator;

    private bool stopMoving = false;
    private bool charging = false;

    private float accel;
    private float deaccel;
    private float maxSpeed;

    private float overflowSpeed;
    private float overflowDeaccel;

    private bool stopTurning;

    private float footstepTime;
    private float landSFXtime;

    // Start is called before the first frame update
    void Start()
    {
        moveInput = GetComponent<PlayerInput>().actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<StatManager>();
        animator = GetComponent<Animator>();

        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");

        overflowSpeed = maxSpeed;
        overflowDeaccel = 0.96f;
        footstepTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stopMoving)
        {
            Movement();
        }

        if (stopMoving && animator.GetBool("p_grounded"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * stats.ComputeValue("Running Deaccel."), GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    /// <summary>
    /// Controlls the basic movement of the player (moving left and right and ground slam)
    /// </summary>
    private void Movement()
    {
        float input = moveInput.ReadValue<float>();
        Vector2 newVel = new Vector2(0, 0);

        // accelerates player in direction of input
        newVel.x = rb.velocity.x + input * accel;

        // caps top horizontal speed, accounting for overflow top speed due to knockback
        if (overflowSpeed > maxSpeed)
        {
            newVel.x = Mathf.Clamp(newVel.x, -1 * overflowSpeed, overflowSpeed);
            overflowSpeed = Mathf.Clamp(Mathf.Abs(rb.velocity.x), maxSpeed, overflowSpeed * overflowDeaccel);
        }
        else
        {
            newVel.x = Mathf.Clamp(newVel.x, -1 * maxSpeed, maxSpeed);
            overflowSpeed = maxSpeed;
        }

        // deaccelerate if no input
        if (input == 0)
        {
            newVel.x *= deaccel;
        }
        if (input!=0 && animator.GetBool("p_grounded"))
        {
            if (Time.time - footstepTime > 0.25f) {
                AudioManager.Instance.SFXBranch.PlaySFXTrack(SFXTrackName.FOOTSTEP);
                footstepTime = Time.time;
            }
        }

        // keep updating y velocity
        newVel.y = rb.velocity.y;

        if (!stopTurning && Mathf.Abs(newVel.x) > 0.1f)
        {
            if (newVel.x < 0f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (newVel.x > 0f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        // update rigidbody velocity to new velocity
        rb.velocity = newVel;
    }

    public void UpdateRun()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }

    public void StartJump()
    {
        accel = stats.ComputeValue("Airborne Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Airborne Deaccel.");
    }

    public void UpdateJump()
    {
        accel = stats.ComputeValue("Airborne Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Airborne Deaccel.");
    }

    public void StopJump()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }

    public void StartFall()
    {
        accel = stats.ComputeValue("Airborne Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Airborne Deaccel.");
    }

    public void UpdateFall()
    {
        accel = stats.ComputeValue("Airborne Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Airborne Deaccel.");
    }

    public void StopFall()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
        if (animator.GetBool("p_grounded"))
        {
            if(Time.time - landSFXtime > 0.25)
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack(SFXTrackName.LANDING);
                landSFXtime = Time.time;
            }
        }
    }

    public void StartFastFall()
    {
        accel = stats.ComputeValue("Airborne Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Airborne Deaccel.");
    }

    public void UpdateFastFall()
    {
        accel = stats.ComputeValue("Airborne Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Airborne Deaccel.");
    }

    public void StopFastFall()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }

    public void StartGlide()
    {
        accel = stats.ComputeValue("Glide Accel.");
        maxSpeed = stats.ComputeValue("Max Glide Speed");
        deaccel = stats.ComputeValue("Glide Deaccel.");
    }

    public void UpdateGlide()
    {
        accel = stats.ComputeValue("Glide Accel.");
        maxSpeed = stats.ComputeValue("Max Glide Speed");
        deaccel = stats.ComputeValue("Glide Deaccel.");
    }

    public void StopGlide()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }

    public void StartDash()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }

    public void StopDash()
    {

    }

    public void StartLightAttack()
    {
        stopTurning = true;
    }

    public void StopLightAttack()
    {
        if (!charging)
        {
            stopTurning = false;
        }
    }

    public void StartHeavyChargeUp()
    {
        charging = true;
        accel = 0;
        maxSpeed = 0;
        //maxSpeed = stats.ComputeValue("Max Speed while Heavy");
        deaccel = stats.ComputeValue("Deaccel. while Heavy");
    }

    public void StopHeavyChargeUp()
    {
        if (charging)
        {
            stopTurning = false;
            charging = false;
            accel = stats.ComputeValue("Running Accel.");
            maxSpeed = stats.ComputeValue("Max Running Speed");
            deaccel = stats.ComputeValue("Running Deaccel.");
            Debug.Log("Stop Heavy ChargeUp");
        }
    }

    public void StartHeavyPrimed()
    {
        Debug.Log("Primed");
        charging = false;
        accel = 0;
        maxSpeed = 0;
        deaccel = stats.ComputeValue("Deaccel. while Heavy");
    }

    public void StopHeavyPrimed()
    {
        stopTurning = false;
        accel = 0;
        maxSpeed = 0;
        deaccel = stats.ComputeValue("Running Deaccel.");
        Debug.Log("Back to normal");
    }

    public void PlayerStop()
    {
        stopMoving = true;
        stopTurning = true;
    }

    public void PlayerGo()
    {
        stopMoving = false;
        stopTurning = false;
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }

    /// <summary>
    /// Knockback the player.
    /// </summary>
    /// <param name="direction">The direction of the knockback</param>
    /// <param name="knockbackStrength">The strength of the knockback</param>
    /// <param name="cancelCurrentMovement">If true, all player movement is overriden by this knockback</param>
    public void ApplyKnockback(Vector2 direction, float knockbackStrength, bool cancelCurrentMovement)
    {
        if (overflowSpeed < knockbackStrength) overflowSpeed = knockbackStrength;
        if (cancelCurrentMovement) rb.velocity = Vector2.zero;
        rb.velocity += direction.normalized * knockbackStrength;
    }
}