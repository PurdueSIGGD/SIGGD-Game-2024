using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostFollowing : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 playerPos;
    [SerializeField]
    float speed = 1.5f;
    void Start()
    {
        playerPos = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = this.transform.position + new Vector3(playerPos.x, playerPos.y, 0) * Time.deltaTime * speed;
        this.transform.position = newPos;
    }
}
