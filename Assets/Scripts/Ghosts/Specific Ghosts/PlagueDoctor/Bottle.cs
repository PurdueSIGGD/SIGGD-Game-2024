using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Bottle : MonoBehaviour, IStatList
{

    [SerializeField]
    private StatManager.Stat[] statList;
    //[SerializeField] private float range;
    [SerializeField] private GameObject blightPrefab;
    private ParticleSystem blightParticles;
    //[SerializeField] private LayerMask attackMask;

    void Start()
    {
        blightParticles = GetComponentInChildren<ParticleSystem>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Vector2 point = transform.position;
        blightParticles.Play();
        blightParticles.transform.parent = transform.parent;
        Destroy(gameObject);
        //Collider2D[] collides = Physics2D.OverlapCircleAll(point, range, attackMask);
        //foreach (Collider2D c in collides)
        //{
        //    GameObject blight = Instantiate(blightPrefab, c.transform);
        //    blight.GetComponent<Blight>().effectParent = c.gameObject;
        //}
    }

    private bool HasParticlesStopped()
    {
        return !blightParticles.isEmitting;
    }

    private IEnumerator SpawnParticles()
    {
        blightParticles.Play();
        blightParticles.transform.parent = transform.parent;
        yield return new WaitUntil(HasParticlesStopped);
        Destroy(gameObject);
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
