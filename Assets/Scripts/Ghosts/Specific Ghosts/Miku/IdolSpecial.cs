using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolSpecial: MonoBehaviour, ISpecialMove
{
    [SerializeField] float maxDistance = 8.0f;
    [SerializeField] GameObject idolClone;

    private bool isDashing;
    private Camera mainCamera;
    private Vector2 dir;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void OnSpecial()
    {
        isDashing = true;
        dir = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Instantiate(idolClone, transform.position, transform.rotation);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxDistance, LayerMask.GetMask("Ground"));
        Vector3 dest;
        if (hit)
        {
            dest = hit.point;
        }
        else
        {
            float destX = transform.position.x + (dir * maxDistance).x;
            float destY = transform.position.y + (dir * maxDistance).y;
            dest = new Vector2(destX, destY);
        }
        transform.position = dest;
        isDashing = false;
    }

    public bool GetBool()
    {
        return isDashing;
    }
}
