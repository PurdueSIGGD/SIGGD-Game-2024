using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    private GameObject playerRespawnPoint;

    private void Start()
    {
        playerRespawnPoint = GameObject.FindGameObjectWithTag("Player Respawn");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = playerRespawnPoint.transform.position;
        }
    }
}
