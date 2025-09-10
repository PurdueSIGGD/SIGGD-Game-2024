using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class FastFall : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float forceStrength = -100.0f;

    private bool isFastFalling = false;
    private bool isStarting = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartStallTimer();
    }

    public void StartStallTimer()
    {
        StartCoroutine(StallTimer());
    }

    private IEnumerator StallTimer()
    {
        isStarting = true;
        yield return new WaitForSeconds(0.5f);
        isStarting = false;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 downwardForce = new Vector2(0.0f, forceStrength);
        if (isFastFalling)
        {
            rb.AddForce(downwardForce);
        }
    }

    public void StartFastFall()
    {
        isFastFalling = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("FastFall");
    }

    public void StopFastFall()
    {
        isFastFalling = false;
        if (GetComponent<Animator>().GetBool("p_grounded") && !isStarting)
        {
            AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Long Fall");
            CameraShake.instance.Shake(0.15f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f)); // add screen shake
        }
        AudioManager.Instance.SFXBranch.StopSFXTrack("FastFall");
    }

    public void StopFastFall(bool justTheBareNecessitiesTheSimpleBearNecessities)
    {
        isFastFalling = false;
        if (GetComponent<Animator>().GetBool("p_grounded") && !isStarting && !justTheBareNecessitiesTheSimpleBearNecessities)
        {
            AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Long Fall");
            CameraShake.instance.Shake(0.15f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f)); // add screen shake
        }
        AudioManager.Instance.SFXBranch.StopSFXTrack("FastFall");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (isFastFalling == true && collision.transform.position.y < transform.position.y)
        {
            CameraShake.instance.Shake(0.15f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f)); // add screen shake
        }
        */
    }

}
