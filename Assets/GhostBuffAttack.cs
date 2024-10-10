using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBuffAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    int buff;

    bool inParty;

    public void EnterParty(GameObject player)  {
        Stat Attack = player.GetComponent<Stats>().GetAttack();
        Attack.ModifyStat(buff);
    }

    public void ExitParty(GameObject player) {
        Stat Attack = player.GetComponent<Stats>().GetAttack();
        Attack.ModifyStat(-1 * buff);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
