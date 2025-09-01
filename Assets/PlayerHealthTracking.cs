using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// TODO, temp script, update
/// </summary>
public class PlayerHealthTracking : MonoBehaviour
{

    static float trackedHealth;
    LevelSwitching levelSwitchingScript;
    StatManager stats;
    PlayerHealth health;

    void UpdateTrackedHealth(float value)
    {
        trackedHealth = value;
    }
    void Start()
    {
        stats = PlayerID.instance.GetComponent<StatManager>();
        health = PlayerID.instance.GetComponent<PlayerHealth>();

        // initialize trackedHealth's static value if it hadn't been initialized previously
        if (trackedHealth == 0)
        {
            UpdateTrackedHealth(health.currentHealth / stats.ComputeValue("Max Health"));
        }

        // set health to maximum if we've just spawned in the hub world
        levelSwitchingScript = FindFirstObjectByType<LevelSwitching>();
        if (levelSwitchingScript && SceneManager.GetActiveScene().name.Equals(levelSwitchingScript.GetHomeWorld()))
        {
            UpdateTrackedHealth(1);
        }

        StartCoroutine(DelayedStart());

        // when door is opened (aka begin changing to next room), make trackedHealth equal to the player's health
        // before exiting the room so that trackedHealth can be used in the next room to update the health 
        Door.OnDoorOpened += TrackPlayerHealth;
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.05f);
        // update player's current health to match the trackedHealth value
        health.currentHealth = stats.ComputeValue("Max Health") * trackedHealth;
    }

    private void TrackPlayerHealth()
    {
        UpdateTrackedHealth(health.currentHealth / stats.ComputeValue("Max Health"));
    }
}
