using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class TransparentPlatdormScript : MonoBehaviour
{

    private Collider2D _collider;
    private bool _playerOnPlatform;
    public GameObject player;
    private Collider2D playercollider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        playercollider = player.GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
           // _collider.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }
    /// <summary>
    /// makes the players collider ignore the platforms collider then stops the code for 1 second in when then it reverts the colliders to normal
    /// </summary>
    private IEnumerator EnableCollider()
    {
        Physics2D.IgnoreCollision(playercollider, _collider);
        yield return new WaitForSeconds(1f);
       // _collider.enabled = true;
       Physics2D.IgnoreCollision(playercollider, _collider, false);
    }
    /// <summary>
    /// This detects if the player is on the platform
    /// </summary>
    /// <param name="other"></param>
    /// <param name="value"></param>
    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
       
        if (player != null)
        {
            _playerOnPlatform = value;
        }
    }
    /// <summary>
    /// Checks to see if an object collides with the platform
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, true);
    }
 /// <summary>
 /// Checks to see if an object extis the platform
 /// </summary>
 /// <param name="other"></param>
    private void OnCollisionExit2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, true);
    }



}
