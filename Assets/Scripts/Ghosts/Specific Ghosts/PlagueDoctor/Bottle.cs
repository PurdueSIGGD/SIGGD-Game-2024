using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Bottle : MonoBehaviour, IStatList
{
    [SerializeField] private StatManager.Stat[] statList;
    [SerializeField] private GameObject blightPrefab;
    private ParticleSystem blightParticles;
    private SpriteRenderer sp;
    private Collider2D colider;
    private Rigidbody2D rb;

    void Start()
    {
        blightParticles = GetComponentInChildren<ParticleSystem>();
        sp = GetComponentInChildren<SpriteRenderer>();
        colider = GetComponentInChildren<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        blightParticles.Play();

        StartCoroutine(DestroyBottle());
    }

    private IEnumerator DestroyBottle()
    {
        sp.enabled = false; // hide the bottle
        colider.enabled = false;
        rb.simulated = false;

        yield return new WaitForSeconds(blightParticles.main.duration);
        Destroy(gameObject);
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
