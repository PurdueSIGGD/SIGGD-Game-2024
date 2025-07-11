using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// TODO, temp script, update
/// </summary>
public class PlayerHealthTracking : MonoBehaviour
{

    static float trackedHealth;
    LevelSwitching levelSwitchingScript;

    // NOTE: redundant code for current implementation of health tracking but may be useful later so I'm leaving it here

    // void OnEnable()
    // {
    //     GameplayEventHolder.OnDeath += PlayerDeathHealthFunction;
    // }
    // void OnDisable()
    // {
    //     GameplayEventHolder.OnDeath -= PlayerDeathHealthFunction;
    // }

    // /// <summary>
    // /// On death, sets player tracked health to max health so that, on next player instantiation,
    // /// sets player health to it's maximum via the value in 'static float health' through the Start function.
    // /// </summary>
    // /// <param name="context"></param>
    // private void PlayerDeathHealthFunction(DamageContext context)
    // {
    //     if (context.victim != gameObject)
    //     {
    //         return;
    //     }
    //     UpdateTrackedHealth(PlayerID.instance.GetComponent<Health>().GetStats().ComputeValue("Max Health"));
    // }

    void UpdateTrackedHealth(float value)
    {
        trackedHealth = value;
    }
    void Start()
    {
        // initialize trackedHealth's static value if it hadn't been initialized previously
        if (trackedHealth == 0)
        {
            UpdateTrackedHealth(PlayerID.instance.GetComponent<Health>().currentHealth);
        }

        // set health to maximum if we've just spawned in the hub world
        levelSwitchingScript = FindFirstObjectByType<LevelSwitching>();
        if (SceneManager.GetActiveScene().name.Equals(levelSwitchingScript.GetHomeWorld()))
        {
            UpdateTrackedHealth(PlayerID.instance.GetComponent<Health>().GetStats().ComputeValue("Max Health"));
        }

        // update player's current health to match the trackedHealth value
        PlayerID.instance.GetComponent<Health>().currentHealth = trackedHealth;

        // when door is opened (aka begin changing to next room), make trackedHealth equal to the player's health
        // before exiting the room so that trackedHealth can be used in the next room to update the health 
        Door.OnDoorOpened += TrackPlayerHealth;
    }
    private void TrackPlayerHealth()
    {
        UpdateTrackedHealth(PlayerID.instance.GetComponent<Health>().currentHealth);
    }
}
