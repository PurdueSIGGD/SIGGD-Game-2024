using UnityEngine;
using static DropTable;

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

    private void DropPlayerSpirits(float dropCount, GameObject spiritPrefab)
    {
        float originalDropCount = dropCount;
        dropCount /= 15f;
        dropCount = Mathf.Min(dropCount, 10f);
        if (originalDropCount > 1f) dropCount = Mathf.Max(dropCount, 2f);

        float xDeviation = -0.0100f;
        float yDeviation = 0.00100f;

        for (int i = 0; i < dropCount; i++)
        {
            Rigidbody2D rb = Instantiate(spiritPrefab, PlayerID.instance.transform.position, PlayerID.instance.transform.rotation).GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(UnityEngine.Random.value * xDeviation, yDeviation), ForceMode2D.Impulse);
            xDeviation = -xDeviation;
        }
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

            if (context.victim.CompareTag("Player"))
            {
                SpiritTracker spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();
                DropPlayerSpirits(spiritTracker.redSpiritsCollected, spiritTracker.redSpiritPrefab);
                DropPlayerSpirits(spiritTracker.blueSpiritsCollected, spiritTracker.blueSpiritPrefab);
                DropPlayerSpirits(spiritTracker.yellowSpiritsCollected, spiritTracker.yellowSpiritPrefab);
                DropPlayerSpirits(spiritTracker.pinkSpiritsCollected, spiritTracker.pinkSpiritPrefab);
                return;
            }

            DropTable table = victim.GetComponent<DropTable>();
            if (!context.victim.CompareTag("Enemy") || table == null) return;
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
                r = Random.Range(0f, 1f);
                int dropCount = Mathf.RoundToInt((drop.maxCount - drop.minCount) * r + drop.minCount);
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
