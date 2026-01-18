using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursorLink : MonoBehaviour
{
    [SerializeField] private GameObject playerCursorPrefab;
    private GameObject playerCursor;
    private PlayerCursorManager playerCursorManager;

    // Start is called before the first frame update
    void Awake()
    {
        playerCursor = Instantiate(playerCursorPrefab);
        playerCursorManager = playerCursor.GetComponent<PlayerCursorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerCursorManager GetPlayerCursorManager()
    {
        return playerCursorManager;
    }
}
