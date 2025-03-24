using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    void OnEnable()
    {
        GameplayEventHolder.OnDeath += DropLoot;
    }

    private void DropLoot(ref DamageContext context)
    {
        GameObject victim = context.victim;
        DropTable table = victim.GetComponent<DropTable>();
        if (!context.attacker.CompareTag("Player") || table == null) { return; }

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
            float dropCount = (int)((drop.maxCount - drop.minCount) * r + drop.minCount);

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
