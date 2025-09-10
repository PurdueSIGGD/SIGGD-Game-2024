using UnityEngine;

public class PinCushion : Skill
{
    [SerializeField] GameObject voodooDoll;
    private SeamstressManager manager;

    void Start()
    {
        GameplayEventHolder.OnAbilityUsed += SummonDoll;
        manager = FindObjectOfType<SeamstressManager>();
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnAbilityUsed -= SummonDoll;
    }

    public void SummonDoll(ActionContext context)
    {
        if (GetPoints() > 0 && context.actionID == ActionID.SEAMSTRESS_SPECIAL && context.extraContext != null && context.extraContext.Equals(""))
        {
            Vector2 summonOrigin = PlayerID.instance.transform.position;
            Vector2 projectForce = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * 450;

            GameObject dollRef = Instantiate(voodooDoll, summonOrigin, transform.rotation);
            dollRef.GetComponent<StatManager>().SetStat("Max Health", GetPoints() * 2000);
            dollRef.GetComponent<Rigidbody2D>().AddForce(projectForce);
            dollRef.AddComponent<FateboundDebuff>().manager = manager;

            manager.AddEnemy(dollRef);

            AudioManager.Instance.VABranch.PlayVATrack("Yume-Seamstress Pin Cushion");
        }
    }

    public override void AddPointTrigger() { }
    public override void RemovePointTrigger() { }
    public override void ClearPointsTrigger() { }
}
