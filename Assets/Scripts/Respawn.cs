using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 respawnPoint;

    private void Start()
    {
        respawnPoint = this.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            this.transform.position = respawnPoint;
        }
    }
}
