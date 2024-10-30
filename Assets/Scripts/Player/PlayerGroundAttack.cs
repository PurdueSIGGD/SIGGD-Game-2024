using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This is the script that contains the code form the player's melee attack
/// </summary>
public class PlayerGroundAtack : MonoBehaviour
{
    [SerializeField] GameObject indicator; //The hitbox for the sword attack

    private BoxCollider2D hittingBox; // Collision Box of Indicator
    [SerializeField] float swordDist; //How far away the sword should rotate from the player
    [SerializeField] int damage; //Damage of Player
    
    private int counter = 0;
    private Camera mainCamera;
    private ContactFilter2D Filter = new ContactFilter2D().NoFilter(); // For Attack Collider Detection
    
    List<Collider2D> colliders = new List<Collider2D>(); // Holds list of colliders of Indicator

    
    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        hittingBox = indicator.GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (indicator.activeSelf)
        {
            if (counter <= 0) {
                indicator.SetActive(false);
                attack();
            }
            counter--;
        }

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 subV = gameObject.transform.position - mousePos;
        float angle = Mathf.Atan2(subV.y, subV.x);
        Vector2 offset = new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle));
        indicator.transform.position = gameObject.transform.position + new Vector3(offset.x, offset.y, 0) * swordDist;
        indicator.transform.Rotate(0, 0, angle * 180/Mathf.PI - indicator.transform.eulerAngles.z);
    }

    private void attack() {
        Collider2D[] collided = Physics2D.OverlapBoxAll(new Vector2(indicator.transform.position.x,indicator.transform.position.y), indicator.transform.localScale, indicator.transform.eulerAngles.z);
        foreach(Collider2D collide in collided) {
           if (collide.transform.CompareTag("Enemy")) {
                collide.GetComponent<EnemyHealth>().TakeDamage(damage);
           }
        }
    }

    /// <summary>
    /// A function that gets called whenever the "Hit" action in Player Actions (currently mouse left)
    /// </summary>
    private void OnHit()
    {
        indicator.SetActive(true);
        counter = 10;
    }
}
