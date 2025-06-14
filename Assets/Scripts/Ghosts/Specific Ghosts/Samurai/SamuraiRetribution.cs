using UnityEngine;

public class SamuraiRetribution : MonoBehaviour
{
    [HideInInspector] public SamuraiManager manager;
    private PlayerStateMachine psm;
    private bool parrying;
    private Camera mainCamera;

    void Start()
    {
        psm = GetComponent<PlayerStateMachine>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (parrying)
        {
            ParryingProjectiles();
        }
    }

    public void StartParry()
    {
        Debug.Log("StartParrying");

        parrying = true;
        GameplayEventHolder.OnDamageFilter.Add(ParryingFilter);
    }

    private void ParryingProjectiles()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = Vector3.zero;
        if (mousePos.x < transform.position.x)
        {
            dir = new Vector3(-1, 0, 0);
        }
        else
        {
            dir = new Vector3(1, 0, 0);
        }

        Collider2D[] coll = Physics2D.OverlapBoxAll(dir + transform.position, new Vector2(2, 3), 0, LayerMask.GetMask("Projectiles", "Enemy"));

        Debug.DrawLine(transform.position + dir * 2 + new Vector3(0, -1.5f, 0), transform.position + dir * 2 + new Vector3(0, 1.5f, 0), Color.blue, Time.deltaTime);
        Debug.DrawLine(transform.position + new Vector3(0, -1.5f, 0), transform.position + new Vector3(0, 1.5f, 0), Color.blue, Time.deltaTime);

        bool parrySuccess = false;

        foreach (Collider2D coll2d in coll)
        {
            if (!coll2d.gameObject.CompareTag("Enemy"))
            {
                EnemyProjectile projectile = coll2d.gameObject.GetComponent<EnemyProjectile>();
                if (projectile && !projectile.parried)
                {
                    projectile.target = "Enemy";
                    projectile.SwitchDirections();
                    projectile.SetParried(true);
                    projectile.projectileDamage.actionID = ActionID.SAMURAI_SPECIAL;
                    parrySuccess = true;
                    NotifyParrySuccess();
                }
            }
        }

        if (parrySuccess) psm.EnableTrigger("finishParry");
    }

    public void StopParry()
    {
        Debug.Log("Stop Parrying"); 
        parrying = false;
        GameplayEventHolder.OnDamageFilter.Remove(ParryingFilter);
    }

    public void ParryingFilter(ref DamageContext context)
    {
        if (context.attacker.CompareTag("Enemy"))
        {
            DamageContext newContext = context;
            newContext.attacker = gameObject;
            newContext.victim = context.attacker;
            newContext.actionID = ActionID.SAMURAI_SPECIAL;
            newContext.damage = context.damage * 
                                manager.GetStats().ComputeValue("Melee Parry Bonus Damage per Incoming Attack Damage") +
                                manager.GetStats().ComputeValue("Melee Parry Base Damage");
            context.attacker.GetComponent<Health>().Damage(newContext, gameObject);
            context.damage = 0;

            NotifyParrySuccess();

            // once a parry is successful, exit parry anim
            psm.EnableTrigger("finishParry");
        }
    }

    private void NotifyParrySuccess()
    {
        ActionContext newContext = manager.onParryContext;
        newContext.extraContext = "Parry Success";
        GameplayEventHolder.OnAbilityUsed?.Invoke(newContext);
    }
}
