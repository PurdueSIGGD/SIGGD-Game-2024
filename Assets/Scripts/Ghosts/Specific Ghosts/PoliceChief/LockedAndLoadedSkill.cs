using UnityEngine;

public class LockedAndLoadedSkill : Skill
{
    int[] reserveCharges = {0, 3, 6, 9, 12};
    private int pointIndex;
    private int reservedCount;
    private float reserveCoolDown = 0.75f;
    private float lastreserveTime = 0.5f;
    private bool hasReserves;
    private Animator camAnim;

    PoliceChiefSpecial policeChiefSpecial;

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoint();
        reservedCount = reserveCharges[pointIndex];
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (policeChiefSpecial == null)
        {
            policeChiefSpecial = PlayerID.instance.GetComponent<PoliceChiefSpecial>();
            return;
        }
        else
        {
            if (policeChiefSpecial.manager.getSpecialCooldown() > 0)
            {
                if (reservedCount <= 0)
                {
                    hasReserves = false;
                }
                else
                {
                    hasReserves = true;
                    if (Time.time - lastreserveTime > reserveCoolDown)
                    {
                        UseReserves();
                    }
                }
            }
        }
        policeChiefSpecial.hasReserves = hasReserves;
    }
    void UseReserves()
    {
        if (camAnim.GetBool("pullBack") == true)
        {
            if (Input.GetMouseButton(0))
            {
                policeChiefSpecial.StartSpecialAttack();
                reservedCount--;
                lastreserveTime = Time.time;
            }
        }
    }
}
