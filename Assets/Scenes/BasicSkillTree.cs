using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicSkillTree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Branch> branchList = new List<Branch>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBranch(Branch branch) {
        branchList.add(branch);
    }

    public void GetBranch() {
        return branchList;
    }
}
