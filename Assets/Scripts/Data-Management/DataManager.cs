using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Function: Create a static instance of this class which will hold a constant float.
/// To use: Create a prefab utilizing this script; data will be carried to each instance.
/// </summary>
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [SerializeField]
    public int count; // placeholder float data

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void setCount(int num) {
        count = num;
        Debug.Log("The counter is now: " + count);
    }

    public int getCount() {
        return count;
    }


    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

}
