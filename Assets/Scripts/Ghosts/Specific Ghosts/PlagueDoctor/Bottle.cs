using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Bottle : MonoBehaviour, IStatList
{

    [SerializeField]
    private StatManager.Stat[] statList;
    [SerializeField] private float range;
    [SerializeField] private GameObject blightPrefab;
    [SerializeField] private LayerMask attackMask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided");
        Vector2 point = transform.position;
        Collider2D[] collides = Physics2D.OverlapCircleAll(point, range, attackMask);
        foreach (Collider2D c in collides)
        {
            Instantiate(blightPrefab, c.transform);
        }
        Destroy(gameObject);
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
