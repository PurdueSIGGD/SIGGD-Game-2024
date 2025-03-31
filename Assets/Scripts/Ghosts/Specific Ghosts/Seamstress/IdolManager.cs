using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolManager : GhostManager
{
    private Queue<GameObject> linkableEnemies;
    private ChainedEnemy head; // will usually be the first enemy hit by Yume's projectile
    private ChainedEnemy tail; // should always point to the end of the list
    private ChainedEnemy ptr;

    /// <summary>
    /// Used to keep track of the all chaind enmeies
    /// </summary>
    class ChainedEnemy
    {
        public GameObject enemy;
        public ChainedEnemy chainedTo;

        public ChainedEnemy() { enemy = null; chainedTo = null; }
    }

    protected override void Start()
    {
        base.Start();
        ptr = head = tail = new ChainedEnemy();
        linkableEnemies = new Queue<GameObject>();
    }

    private void ClearList()
    {
        while (ptr.enemy != null)
        {
            Destroy(ptr.enemy.GetComponent<FateboundDebuff>());
            ptr = ptr.chainedTo;
        }
        ptr = head = tail = new ChainedEnemy();
    }
}
