using UnityEngine;

public class Casing : MonoBehaviour
{
    public float startForce;
    public AudioClip[] casingHitGround;

    private AudioSource casingSourse;
    private Rigidbody rb;
    private bool played;

    private void Start()
    {
        casingSourse = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * startForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!played && !collision.transform.CompareTag("Pistol") && !collision.transform.CompareTag("Player"))
        {
            casingSourse.clip = casingHitGround[Random.Range(0, casingHitGround.Length)];
            casingSourse.Play();
            played = true;
            Destroy(gameObject, 10f);
        }
    }
}
