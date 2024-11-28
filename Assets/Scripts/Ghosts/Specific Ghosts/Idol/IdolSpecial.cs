using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolSpecial: MonoBehaviour, ISpecialMove
{
    [SerializeField] float maxDistance = 8.0f;
    [SerializeField] float tpCoolDown = 8.0f; // cooldown for teleporting special action
    [SerializeField] float switchCoolDown = 0.5f; // cooldown for switching with clones
    
    private bool isDashing; // bool for if is currently dashing
    private bool canTp = true;
    private bool canSwitch = false;
    private Camera mainCamera;
    private Vector2 dir;

    private GameObject idolClone; // ref to clone prefab
    private GameObject activeClone; // ref to currently active clone, if exists

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        idolClone = Resources.Load<GameObject>("IdolClone");
    }

    void OnSpecial()
    {
        if (canSwitch) // if there is currently a clone active, switch with it
        {
            StartCoroutine(SwitchCoroutine());
        }
        else if (canTp) // else create clone and teleport
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public bool GetBool()
    {
        return isDashing;
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        canTp = false;
        dir = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        // create a clone
        activeClone = Instantiate(idolClone, transform.position, transform.rotation);
        activeClone.GetComponent<IdolClone>().Initialize(gameObject);
        canSwitch = true;

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

        yield return new WaitForSeconds(tpCoolDown);
        canTp = true;
    }

    private IEnumerator SwitchCoroutine()
    {
        canSwitch = false;
        // check if clone still exists
        if (activeClone == null)
        {
            // if not, dash instead
            StartCoroutine(DashCoroutine());
        }
        else
        {
            // if so, switch places with clone
            (transform.position, activeClone.transform.position) = (activeClone.transform.position, transform.position);
            yield return new WaitForSeconds(switchCoolDown);
            canSwitch = true;
        }
    }
}
