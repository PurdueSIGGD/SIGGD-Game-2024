using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a skill tree that consists of multiple skill tieres 
/// </summary>
public abstract class SkillTree : MonoBehaviour
{
    protected SkillTier[] tierList = new SkillTier[3];
    protected FinalSkill finalSkill;
    protected readonly int TRUST_LEVEL_1 = 25;
    protected readonly int TRUST_LEVEL_2 = 50;
    protected readonly int TRUST_LEVEL_3 = 75;
    protected readonly int FINAL_TRUST_LEVEL = 100; 
    protected int trust;

    protected void Awake()
    {
        trust = 0;
    }

    public void UpdateProgression() {
        if (trust >= TRUST_LEVEL_1) {
            this.tierList[0].Unlock();
            if (trust >= TRUST_LEVEL_2) {
                this.tierList[1].Unlock();
                if (trust >= TRUST_LEVEL_3) {
                    this.tierList[2].Unlock();
                    if (trust == FINAL_TRUST_LEVEL) {
                        //finalSkill.SetUnlock(true);
                    }
                }
            }
        }
    }

    public SkillTier[] GetSkillTiers() {
        return tierList;
    }

    public FinalSkill GetFinalSkill() {
        return finalSkill;
    }

    public void SetFinalSkill(FinalSkill final) {
        finalSkill = final;
    }

    public void SetSkillTier(int index, SkillTier tier)
    {
        this.tierList[index] = tier;
    }

    /// <summary>
    /// Increases trust with a ghost, and then potentially unlocks skill tiers according to current trust level;
    /// </summary>
    /// <param name="amount">Amount of trust to increase by</param>
    public void IncreaseTrust(int amount) {
        trust += amount;
        UpdateProgression();
        Debug.Log(trust);
    }
}
