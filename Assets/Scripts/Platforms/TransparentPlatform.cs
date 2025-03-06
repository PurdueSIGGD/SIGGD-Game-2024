using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Code for platform that you can jump through and fall through
/// </summary>
public class TransparentPlatform : MonoBehaviour
{

    private Collider2D _collider;
    private bool _playerOnPlatform;
    private GameObject player;
    private Collider2D playercollider;
    private InputAction fallAction;
    private PlatformEffector2D effector;
    private IEnumerator coroutine;
    private bool isColliding;

    private void Start()
    {
        //Gets player object
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            print("ERROR: no player tag in scene");
        }

        _collider = GetComponent<Collider2D>();
        playercollider = player.GetComponent<Collider2D>();
        fallAction = GetComponent<PlayerInput>().actions.FindAction("Fall");
        effector = GetComponent<PlatformEffector2D>();
        coroutine = null;
        isColliding = false;
    }

    private void Update()
    {
        if (coroutine == null && fallAction.ReadValue<float>() != 0)
        {
            coroutine = DisableCollider();
            StartCoroutine(coroutine);
        }
    }

    
    /// <summary>
    /// makes the players collider ignore the platforms collider then stops the code for 1 second in when then it reverts the colliders to normal
    /// </summary>
    private IEnumerator DisableCollider()
    {
        effector.surfaceArc = 0;
        while (isColliding)
        {
            yield return null;
        }
        // _collider.enabled = true;
        effector.surfaceArc = 180;
        coroutine = null;
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
