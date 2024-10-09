using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// a skill tree that consists of multiple branches 
/// </summary>
public abstract class BasicSkillTree : MonoBehaviour
{
    // Start is called before the first frame update
    protected List<Branch> branchList;
    void Start()
    {
       branchList = new List<Branch>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// add a branch in the skill tree
    /// </summary>
    /// <param name="branch"> the branch that is added to the skill tree  </param>    
    public void AddBranch(Branch branch) {
        branchList.Add(branch);
    }

    public List<Branch> GetBranches() {
        return branchList;
    }
}
