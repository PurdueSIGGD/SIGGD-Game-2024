using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a skill tree that consists of multiple skill tieres 
/// </summary>
public abstract class BasicSkillTree : MonoBehaviour
{
    protected SkillTier[] tierList = new SkillTier[3];
    protected FinalSkill finalSkill;
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
            this.tierList[0].SetUnlock(true);
            if (trust >= TRUST_LEVEL_2) {
                this.tierList[1].SetUnlock(true);
                if (trust >= TRUST_LEVEL_3) {
                    this.tierList[2].SetUnlock(true);
                    if (trust == FINAL_TRUST_LEVEL) {
                        finalSkill.SetUnlock(true);
                    }
                }
            }
        }
    }

    public SkillTier[] GetSkillTieres() {
        return tierList;
    }

    public FinalSkill GetFinalSkill() {
        return finalSkill;
    }

    public void SetFinalSkill(FinalSkill final) {
        finalSkill = final;
    }

    public void IncreaseTrust(int amount) {
        trust += amount;
        UpdateProgression();
        Debug.Log(trust);
    }
}
