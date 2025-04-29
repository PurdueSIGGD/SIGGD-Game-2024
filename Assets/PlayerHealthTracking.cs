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
        Door.OnDoorOpened += trackPlayerHealth;
    }

    private void trackPlayerHealth()
    {
        health = PlayerID.instance.GetComponent<Health>().currentHealth;
    }
}
