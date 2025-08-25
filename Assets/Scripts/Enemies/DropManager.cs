using UnityEngine;

public class DropManager : MonoBehaviour
{
    void OnEnable()
    {
        GameplayEventHolder.OnDeath += DropLoot;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath -= DropLoot;
    }

    private void DropLoot(DamageContext context)
    {
        Debug.Log(context.victim.name + " Died");
        if (context.victim != context.attacker)
        {
            GameObject victim = context.victim;
            if (victim == null)
            {
                return;
            }
            DropTable table = victim.GetComponent<DropTable>();
            if (!context.victim.CompareTag("Enemy") || table == null) { return; }

            foreach (DropTable.Drop drop in table.dropTable)
            {
                // Decide if each loot will drop
                float r = UnityEngine.Random.value;
                if (r > drop.chance)
                {
                    continue;
                }

                // Decdie how much to drop
                r = UnityEngine.Random.value;
                int dropCount = (int)((drop.maxCount - drop.minCount) * r + drop.minCount);
                dropCount = Mathf.RoundToInt(dropCount * PlayerID.instance.GetComponent<PlayerBuffStats>().GetStats().ComputeValue("Spirit Drop Rate Boost"));

                float xDeviation = -0.0003f;
                float yDeviation = 0.00003f;

                for (int i = 0; i < dropCount; i++)
                {
                    Rigidbody2D rb = Instantiate(drop.obj, victim.transform.position, victim.transform.rotation).GetComponent<Rigidbody2D>();
                    rb.AddForce(new Vector2(UnityEngine.Random.value * xDeviation, yDeviation), ForceMode2D.Impulse);
                    xDeviation = -xDeviation;
                }
            }
        }
    }
}
