using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using JetBrains.Annotations;

public class SpiritFollowEntity : MonoBehaviour {
     [SerializeField]
    GameObject target; // Which entity is being followed

    [SerializeField]
    float orbitDistance;
    [SerializeField]
	public float orbitDegreesPerSec;

    Rigidbody2D rb;

    /// <summary>
    /// Calculates and adds the steering force that will push this gameObject towards the target's position.
    /// </summary>
    private void Steer()
    {
        if (target == null) return;
        transform.position = target.transform.position + (transform.position - target.transform.position).normalized * orbitDistance;
		transform.RotateAround(target.transform.position, Vector3.forward, orbitDegreesPerSec * Time.deltaTime);
		}


    // Start is called before the first frame update
    void Start() {
        orbitDegreesPerSec = Random.Range(30, 90);
        orbitDistance = Random.Range(3, 17);
        EnterParty(target);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Steer();
    }

    public void EnterParty(GameObject player)
    {
        this.target = player;
    }

    public void ExitParty(GameObject player)
    {
        this.target = null;
        this.rb.velocity = new Vector2(0, 0);
    }

}