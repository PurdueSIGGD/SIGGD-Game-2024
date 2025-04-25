using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : Skill
{
    IdolManager idolManager;
    GameObject player;
    IdolSpecial idolSpecial;

    [SerializeField] private float invisibilityDuration = 3f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        idolManager = gameObject.GetComponent<IdolManager>();
        idolManager.evaSelectedEvent.AddListener(EvaSelected);
    }


    private void EvaSelected()
    {
        idolSpecial = idolManager.special;
        idolSpecial.holoJumpCreatedCloneEvent.AddListener(HoloJumpCreatedClone);
    }

    private void HoloJumpCreatedClone()
    {
        if (player.GetComponent<Invisible>() == null)
        {
            player.AddComponent<Invisible>();
            StartCoroutine(removeInvisibility(invisibilityDuration));
        }
    }

    private IEnumerator removeInvisibility(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(player.GetComponent<Invisible>());
    }

    public override void AddPointTrigger()
    {
        
    }
    public override void RemovePointTrigger()
    {
        
    }
    public override void ClearPointsTrigger()
    {
        
    }
}
