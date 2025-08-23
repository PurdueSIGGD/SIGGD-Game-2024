using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerBox : MonoBehaviour
{
    [SerializeField] bool activeOnStart;
    [SerializeField] ConvoSO convo;
    private bool active;
    private bool colliding;

    void Start()
    {
        active = activeOnStart;
    }

    void Update()
    {
        if (!activeOnStart && EnemySpawning.instance.roomCleared)
        {
            active = true;
        }
        if (colliding && active)
        {
            DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
            dialogueManager.StartDialogue(convo);
            this.enabled = false;
        }
    }

    public void SetConvo(ConvoSO convo)
    {
        this.convo = convo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerID.instance.gameObject)
        {
            colliding = true;
        }
    }
}
