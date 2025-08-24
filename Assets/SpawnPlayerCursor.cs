using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerCursor : MonoBehaviour
{
    [SerializeField] private GameObject playerCursor;


    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerCursor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
