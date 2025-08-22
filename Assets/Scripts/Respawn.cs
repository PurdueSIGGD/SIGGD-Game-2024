using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 respawnPoint;
    [SerializeField] private bool dealDmg;
    [SerializeField] DamageContext damageContext;
    [SerializeField] float damage;

    private void Start()
    {
        respawnPoint = this.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            this.transform.position = respawnPoint;
            if (dealDmg)
            {
                if(GetComponent<Health>().currentHealth > damage)
                {
                    damageContext.damage = damage;
                    //GetComponent<Health>().Damage(damageContext, gameObject);
                    GetComponent<Health>().currentHealth -= damage;
                }
                else
                {
                    damageContext.damage = GetComponent<Health>().currentHealth - 1;
                    GetComponent<Health>().currentHealth = 1;
                }
            }
            
        }
    }
}
