using UnityEngine;
using UnityEngine.SceneManagement;

public class LockedAndLoadedSkill : Skill
{
    [SerializeField] public int[] reserveCharges = {0, 3, 6, 9, 12};
    [HideInInspector] public int pointIndex;
    [HideInInspector] public int reservedCount = -1;

    LevelSwitching levelSwitchingScript;

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
        levelSwitchingScript = FindFirstObjectByType<LevelSwitching>();
        if (SceneManager.GetActiveScene().name.Equals(levelSwitchingScript.GetHomeWorld()))
        {
            reservedCount = reserveCharges[pointIndex];
            SaveManager.data.north.reserveSpecialCharges = reservedCount;
        }
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
        levelSwitchingScript = FindFirstObjectByType<LevelSwitching>();
        if (SceneManager.GetActiveScene().name.Equals(levelSwitchingScript.GetHomeWorld()))
        {
            reservedCount = reserveCharges[pointIndex];
            SaveManager.data.north.reserveSpecialCharges = reservedCount;
        }
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
        levelSwitchingScript = FindFirstObjectByType<LevelSwitching>();
        if (SceneManager.GetActiveScene().name.Equals(levelSwitchingScript.GetHomeWorld()))
        {
            reservedCount = reserveCharges[pointIndex];
            SaveManager.data.north.reserveSpecialCharges = reservedCount;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        reservedCount = SaveManager.data.north.reserveSpecialCharges;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ConsumeReserveCharge()
    {
        reservedCount = Mathf.Max(reservedCount - 1, 0);
        SaveManager.data.north.reserveSpecialCharges = reservedCount;
    }
}
