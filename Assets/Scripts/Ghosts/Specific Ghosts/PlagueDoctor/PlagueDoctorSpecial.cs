using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlagueDoctorSpecial : MonoBehaviour
{

    [SerializeField] GameObject blightPotion;
    [SerializeField] float initialSpeed;
    [SerializeField] Vector2 bottleOffset;

    // TODO, replace these when assembling ghost
    [SerializeField] float cooldown;
    private float curCd;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        curCd -= Time.deltaTime;
    }

    void StartDash()
    {
        if (curCd > 0) { return; }
        GameObject bottle = Instantiate(blightPotion, (Vector2)transform.position + bottleOffset, transform.rotation);
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = PlayerID.instance.transform.position;
        bottle.GetComponent<Rigidbody2D>().velocity = (mousePos - playerPos).normalized * initialSpeed;
        PlayerStateMachine psm = this.GetComponent<PlayerStateMachine>();
        psm.EnableTrigger("OPT");
        curCd = cooldown;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottleOffset, 0.3f);
    }
}
