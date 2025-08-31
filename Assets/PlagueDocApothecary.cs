using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlagueDocApothecary : MonoBehaviour
{
    public bool isHealing = false;
    public float timer = 0f;

    [HideInInspector] public SilasManager manager;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Apothecary Timer: " + timer);
        if (isHealing)
        {
            Debug.Log("Apothecary Timer: " + timer);
            timer -= Time.deltaTime;

            // Heal player and consume ingredients
            if (timer <= 0f)
            {
                manager.SetIngredientsCollected(0);
                GetComponent<Health>().Heal(manager.basicHealing, PlayerID.instance.gameObject);
                GetComponent<PlayerStateMachine>().EnableTrigger("OPT");

                //VFX
                GameObject explosion = Instantiate(manager.bombExplosionVFX, transform.position, Quaternion.identity);
                explosion.GetComponent<RingExplosionHandler>().playRingExplosion(2.5f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

                // SFX
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Silas-Apothecary Use");
                AudioManager.Instance.VABranch.PlayVATrack("Silas-PlagueDoc Apothecary");

                // Apply Self-medicated Buff
                SelfMedicated selfMedicated = manager.GetComponent<SelfMedicated>();
                if (selfMedicated.isBuffed)
                {
                    selfMedicated.SetBuffTime(selfMedicated.apothecaryBuffDuration);
                }
                else
                {
                    selfMedicated.ApplyBuff(selfMedicated.apothecaryBuffDuration);
                }
            }
        }

    }



    void StartCrouch()
    {
        GetComponent<PartyManager>().SetSwappingEnabled(false);
        GetComponent<Move>().PlayerStop();
        timer = manager.GetStats().ComputeValue("Basic Cast Time");
        isHealing = true;
        Debug.Log("Entering Apothecary");
    }

    void StopCrouch()
    {
        GetComponent<PartyManager>().SetSwappingEnabled(true);
        GetComponent<Move>().PlayerGo();
        isHealing = false;
        Debug.Log("Exiting Apothecary");
    }


}
