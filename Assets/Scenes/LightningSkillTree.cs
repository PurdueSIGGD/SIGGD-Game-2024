using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkillTree : SkillTree
{
    // Start is called before the first frame update
    void Start()
    {
        float[] sk1Nums = { 0.0f, 5.0f, 10.0f, 15.0f, 20.0f, 23.0f, 25.0f };
        Skill sk1 = new Skill("Increased Damage", "Increases the damage by XXX.", sk1Nums);
        sk1.effect = number =>
        {
            Debug.Log("Damage increased to " + number);
        };

        float[] sk2Nums = { 0.0f, 5.0f, 10.0f, 15.0f, 20.0f, 23.0f, 25.0f };
        Skill sk2 = new Skill("Increased Speed", "Increases the speed by XXX.", sk2Nums);
        sk2.effect = number =>
        {
            Debug.Log("Speed increased to " + number);
        };


        float[] sk3Nums = { 0.0f, 5.0f, 10.0f, 15.0f, 20.0f, 23.0f, 25.0f };
        Skill sk3 = new Skill("Increased Damage", "Increases the damage by XXX.", sk3Nums);
        sk3.effect = number =>
        {
            Debug.Log("Damage increased to " + number);
        };


        float[] sk4Nums = { 0.0f, 5.0f, 10.0f, 15.0f, 20.0f, 23.0f, 25.0f };
        Skill sk4 = new Skill("Increased Damage", "Increases the damage by XXX.", sk4Nums);
        sk4.effect = number =>
        {
            Debug.Log("Damage increased to " + number);
        };


        float[] sk5Nums = { 0.0f, 5.0f, 10.0f, 15.0f, 20.0f, 23.0f, 25.0f };
        Skill sk5 = new Skill("Increased Damage", "Increases the damage by XXX.", sk5Nums);
        sk5.effect = number =>
        {
            Debug.Log("Damage increased to " + number);
        };
    }
}
