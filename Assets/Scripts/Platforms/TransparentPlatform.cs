using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

/// <summary>
/// Code for platform that you can jump through and fall through
/// </summary>
public class TransparentPlatform : MonoBehaviour
{
    private InputAction fallAction;
    private PlatformEffector2D effector;
    private BoxCollider2D collider;
    private IEnumerator coroutine;
    private bool isColliding;
    private GameObject playacol;
    private Rigidbody2D rb;

    private void Start()
    {
        fallAction = GetComponent<PlayerInput>().actions.FindAction("Fall");
        effector = GetComponent<PlatformEffector2D>();
        collider = GetComponent<BoxCollider2D>();
        coroutine = null;
        isColliding = false;
        playacol = GameObject.FindWithTag("Player");
        rb = PlayerID.instance.GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        // TODO: UNCOMMENT THIS!!!!!!!!!!!
        
        if (coroutine == null && fallAction.ReadValue<float>() != 0 && isColliding)
        {
            coroutine = DisableCollider();
            StartCoroutine(coroutine);
        }
        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemycol.GetComponent<Collider2D>(), false);
    }


    /// <summary>
    /// makes the players collider ignore the platforms collider then stops the code for 1 second in when then it reverts the colliders to normal
    /// </summary>
    //private IEnumerator DisableCollider()
    //{
    //    effector.surfaceArc = 0;
    //    while (isColliding)
    //    {
    //        yield return null;
    //    }
    //    effector.surfaceArc = 170;
    //    coroutine = null;
    //}

    private IEnumerator DisableCollider()
    {
        // ignore collision
        Physics2D.IgnoreCollision(playacol.GetComponent<Collider2D>(), GetComponent<CompositeCollider2D>(), true);

        yield return new WaitUntil(checkIfColliding);

        // turning back collision
        Physics2D.IgnoreCollision(playacol.GetComponent<Collider2D>(), GetComponent<CompositeCollider2D>(), false);
        coroutine = null;
    }

    private bool checkIfColliding()
    {
        return rb.velocity.y > 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isColliding = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isColliding = false;
        }
    }
}
