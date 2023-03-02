using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private int damage, bulletSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.transform.CompareTag("Player") && !collision.transform.CompareTag("Gun"))
        {
            if (collision.rigidbody != null)
            {
                collision.rigidbody.AddForce(collision.transform.forward + transform.forward * rb.velocity.magnitude * 0.1f, ForceMode.Impulse);
            }
            if (collision.transform.CompareTag("Enemy"))
            {
                collision.transform.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
//            EffectManager.instance.SpawnBulletHole(collision);
            Destroy(gameObject);
        }
    }
    
    public void SetDamage(int dmg)
    {
        damage= dmg;
    }
    public void SetSpeed(int speed)
    {
        bulletSpeed = speed;
    }

}
