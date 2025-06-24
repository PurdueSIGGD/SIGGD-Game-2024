using UnityEngine;


/// <summary>
/// TODO, temp script, update
/// </summary>
public class PlayerHealthTracking : MonoBehaviour
{
    static float health;

    void Start()
    {
        if (health == 0)
        {
            health = PlayerID.instance.GetComponent<Health>().currentHealth;
        }
        PlayerID.instance.GetComponent<Health>().currentHealth = health;
        Door.OnDoorOpened += TrackPlayerHealth;
        GameplayEventHolder.OnDamageDealt += PlayHurtExertion;
    }

    private void TrackPlayerHealth()
    {
        health = PlayerID.instance.GetComponent<Health>().currentHealth;
    }
    
    // Play voice line for when player is damaged
    // Keeping this code here cus I don't really want to make a new script just yet
    private void PlayHurtExertion(DamageContext context)
    {
        if (context.victim.CompareTag("Player"))
        {
            // if light amount of damage
            if (context.damage <= 30)
            {
                AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Light Damage Taken");
            }

            // if heavy damage taken
            if (context.damage > 30)
            {
                AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Significant Damage Taken");
            }
        }
    }
}
