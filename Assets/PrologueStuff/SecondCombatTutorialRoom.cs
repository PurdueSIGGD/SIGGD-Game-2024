using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondCombatTutorialRoom : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies;

    void Awake()
    {
        PlayerID.instance.GetComponent<Animator>().SetBool("CanAttack", true);
        PlayerID.instance.GetComponent<Animator>().SetBool("CanHeavyAttack", true);
        PlayerID.instance.GetComponent<StatManager>().SetStat("Max Health", 1000000000);
    }

    void Start()
    {
        Door.OnDoorOpened += SetNextRoom;
        GameplayEventHolder.OnDeath += CheckRemainingEnemies;
    }

    private void OnDisable()
    {
        Door.OnDoorOpened -= SetNextRoom;
        GameplayEventHolder.OnDeath -= CheckRemainingEnemies;
    }

    private void CheckRemainingEnemies(DamageContext context)
    {
        enemies.Remove(enemies[0]);
        if (enemies.Count == 0)
        {
            Door.activateDoor(true);
        }
    }

    private void SetNextRoom()
    {
        StartCoroutine(DoTheThing());
    }

    private IEnumerator DoTheThing()
    {
        ScreenFader.instance.FadeOut();
        yield return new WaitForSeconds(ScreenFader.instance.fadeOutDuration + 0.1f);
        SceneManager.LoadScene("Prologue_4");
    }
}
