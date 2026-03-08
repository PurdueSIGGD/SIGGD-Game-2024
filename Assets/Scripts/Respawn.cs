using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 respawnPoint;
    [SerializeField] private bool dealDmg;
    [SerializeField] DamageContext damageContext;
    [SerializeField] float damage;

    [SerializeField] GameObject pulseVFX;
    [SerializeField] Color pulseColor;

    private void Start()
    {
        respawnPoint = this.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            this.transform.position = respawnPoint;
            GameObject pulseRing = Instantiate(pulseVFX, gameObject.transform.position, Quaternion.identity);
            pulseRing.GetComponent<RingExplosionHandler>().playRingExplosion(20f, pulseColor);
            AudioManager.Instance.SFXBranch.PlaySFXTrack("AirAttack");
            if (dealDmg)
            {
                if (GetComponent<Health>().currentHealth > damage)
                {
                    damageContext.damage = damage;
                    GetComponent<Health>().Damage(damageContext, collision.gameObject);
                    //GetComponent<Health>().currentHealth -= damage;
                }
                else
                {
                    damageContext.damage = GetComponent<Health>().currentHealth - 1;
                    GetComponent<Health>().Damage(damageContext, collision.gameObject);
                    //GetComponent<Health>().currentHealth = 1;
                }
            }

        }
    }

    public void SetRespawnPoint()
    {
        respawnPoint = this.transform.position;
    }
}
