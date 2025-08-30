using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreprologueTextMove : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogue;

    private RectTransform rectTransform;

    private bool start = false;
    public float waitTime;
    public float travelSpeed;

    private void Start()
    {
        StartCoroutine(Waiting());
        rectTransform = dialogue.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (start)
        {
            rectTransform.position = new Vector2(rectTransform.position.x,
                        rectTransform.position.y + travelSpeed * Time.deltaTime);
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        start = true;
    }
}
