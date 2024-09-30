using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This code allows players to jump through a platform and land on it while also allowing them
/// to go through it to get down.
/// </summary>
public class TransparentPlatdormScript : MonoBehaviour
{
    private Collider2D _collider;
    private bool _playerOnPlatform;
    public GameObject player; // gets the game object of the player

    private void Start()
    {
        _collider = GetComponent<Collider2D>();

    }

    private void Update()
    {
        if (_playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            //_collider.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }
    /// <summary>
    /// has the players collider igonore the platform collider and then pauses the
    /// code for 1 second in which afterwards the colliders are back to normal.
    /// </summary>
    private IEnumerator EnableCollider()
    {
        Physics2D.IgnoreCollision(player, _collider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(player, _collider, false);
      //  _collider.enabled = true;
    }
    /// <summary>
    /// Detects if the player is on the platform
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, true);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, true);
    }



}
