using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a skill tree that consists of multiple branches 
/// </summary>
public abstract class BasicSkillTree : MonoBehaviour
{
    protected Branch[] branchList = new Branch[4];
    protected readonly  int TRUST_LEVEL_1 = 25;
    protected readonly  int TRUST_LEVEL_2 = 50;
    protected readonly  int TRUST_LEVEL_3 = 75;
    protected readonly  int FINAL_TRUST_LEVEL = 100; 
    private int trust;

    public BasicSkillTree(int trust) {
        this.trust = trust;
    }

    // Start is called before the first frame update
    protected void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {       
        
    }

    public void UpdateProgression() {
        if (trust >= TRUST_LEVEL_1) {
            this.branchList[0].SetUnlock(true);
            if (trust >= TRUST_LEVEL_2) {
                this.branchList[1].SetUnlock(true);
                if (trust >= TRUST_LEVEL_3) {
                    this.branchList[2].SetUnlock(true);
                    if (trust == FINAL_TRUST_LEVEL) {
                        this.branchList[3].SetUnlock(true);
                    }
                }
            }
        }
    }

    public Branch[] GetBranches() {
        return branchList;
    }

    public void IncreaseTrust(int amount) {
        trust += amount;
        UpdateProgression();
    }
}
