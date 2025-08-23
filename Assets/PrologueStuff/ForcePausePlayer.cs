using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePausePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(PausePlayer());
    }

    private IEnumerator PausePlayer()
    {
        PlayerID.instance.GetComponent<Respawn>().SetRespawnPoint();
        PlayerID.instance.GetComponent<Move>().PlayerStop();
        PlayerID.instance.GetComponent<Animator>().SetBool("Can_Walk", false);
        yield return new WaitForSeconds(0.5f);
        PlayerID.instance.GetComponent<Animator>().SetBool("Can_Walk", true);
        PlayerID.instance.GetComponent<Move>().PlayerGo();
    }
}
