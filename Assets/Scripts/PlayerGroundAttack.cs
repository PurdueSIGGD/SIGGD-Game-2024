using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This is the script that contains the code form the player's melee attack
/// </summary>
public class PlayerGroundAtack : MonoBehaviour
{
    [SerializeField] GameObject indicator; //The hitbox for the sword attack
    [SerializeField] float swordDist; //How far away the sword should rotate from the player
    private int counter = 0;
    private Camera mainCamera;

    
    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (indicator.activeSelf)
        {
            if (counter <= 0) {
                indicator.SetActive(false);
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

    /// <summary>
    /// A function that gets called whenever the "Hit" action in Player Actions (currently mouse left)
    /// </summary>
    private void OnHit()
    {
        indicator.SetActive(true);
        counter = 5;
    }
}
