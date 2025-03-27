using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour, IStatList
{

    [SerializeField]
    private StatManager.Stat[] statList;
    private float range;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D point = collision.GetContact(0);
        Collider2D[] collides = Physics2D.OverlapCircleAll(point.point, range);
        foreach (Collider2D c in collides)
        {
            c.gameObject.AddComponent<Blight>();
        }
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
