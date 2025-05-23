using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class FastFall : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float forceStrength = -100.0f;

    //private CameraShake cameraShaker;
    private bool isFastFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //cameraShaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
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
        AudioManager.Instance.SFXBranch.PlaySFXTrack(SFXTrackName.FAST_FALL);
    }

    public void StopFastFall()
    {
        isFastFalling = false;
        AudioManager.Instance.SFXBranch.StopSFXTrack(SFXTrackName.FAST_FALL);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFastFalling == true && collision.transform.position.y < transform.position.y)
        {
            CameraShake.instance.Shake(0.15f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f)); // add screen shake
        }
    }

}
