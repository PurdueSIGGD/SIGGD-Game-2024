using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Purpose: To relay data between different scenes. Test scenes are located in
/// Scenes/Data-Management. 
/// 
/// Objective: Create a static instance in which two scenes will change its value.
/// </summary>
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

}
