using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreprologueTextMove : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogue;

    private bool start = false;
    public float waitTime;
    public float travelSpeed;

    private void Start()
    {
        StartCoroutine(Waiting());
    }

    void Update()
    {
        if (start)
        {
            dialogue.transform.position = new Vector2(dialogue.transform.position.x,
                        dialogue.transform.position.y + travelSpeed * Time.deltaTime);
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        start = true;
    }
}
