using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody != null)
        {
            collision.rigidbody.AddForce(collision.transform.forward + transform.forward * rb.velocity.magnitude * 0.1f, ForceMode.Impulse);
        }
        EffectManager.instance.SpawnBulletHole(collision);
        Destroy(gameObject);
    }

}
