using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private int damage;
    private float bulletSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.rigidbody != null)
        {
            collision.rigidbody.AddForce(collision.transform.forward + transform.forward * rb.velocity.magnitude * 0.1f, ForceMode.Impulse);
        }
        if (collision.transform.GetComponent<BodyPart>() != null)
        {
            collision.transform.GetComponent<BodyPart>().TakeDamage(damage, damage, (collision.transform.position - transform.position) * 20);

            if (collision.transform.GetComponent<BodyPart>().crit)
            {
                EffectManager.instance.SpawnBulletHole(collision.transform, collision.contacts[0].point, collision.contacts[0].normal, 2);
            }
            else if (collision.transform.GetComponent<BodyPart>() != null && collision.transform.GetComponent<BodyPart>().bodyPart == 1)
            {
                EffectManager.instance.SpawnBulletHole(collision.transform, collision.contacts[0].point, collision.contacts[0].normal, 3);
            }
            else if (collision.transform.GetComponent<BodyPart>() != null)
            {
                EffectManager.instance.SpawnBulletHole(collision.transform, collision.contacts[0].point, collision.contacts[0].normal, 1);
            }
        }
        else if(collision.transform.GetComponent<PlayerHealth>() != null || collision.transform.GetComponent<Camera>() != null)
        {
            GameManager.instance.XROrigin.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        else
        {
            EffectManager.instance.SpawnBulletHole(collision.transform, collision.contacts[0].point, collision.contacts[0].normal, 5);
        }

        Destroy(gameObject);
    }
    
    public void SetDamage(int dmg)
    {
        damage= dmg;
    }
    public void SetSpeed(float speed)
    {
        bulletSpeed = speed;
    }

}
