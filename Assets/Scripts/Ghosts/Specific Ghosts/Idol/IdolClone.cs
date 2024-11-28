using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolClone : MonoBehaviour
{
    [SerializeField] float duration = 6.0f;

    private GameObject player;

    void Update()
    {
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
        if (player.GetComponent<IdolSpecial>())
        {
            duration -= Time.deltaTime;
        }
        else // if player is no longer in idol mode, count down twice as fast
        {
            duration -= Time.deltaTime * 2;
        }
    }

    public void Initialize(GameObject player)
    {
        this.player = player;
    }
}
