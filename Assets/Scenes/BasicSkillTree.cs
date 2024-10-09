using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void AddBranch(Branch branch) {
        branchList.Add(branch);
    }

    public List<Branch> GetBranches() {
        return branchList;
    }
}
