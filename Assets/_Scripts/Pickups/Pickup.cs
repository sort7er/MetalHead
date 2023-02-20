using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int value;
    public float lifeTime, startFlashTime, smoothTime;
    public MeshRenderer pickupMesh;
    public ParticleSystem effect;
    public AudioClip[] metalSounds;

    private AudioSource pickupSource;
    private Transform magnet;
    private Vector3 smallSize;
    private Rigidbody rb;
    private Animator pickUpAnim;
    private float timer, flashTime;
    private bool flashStarted, pickUp, isPickedUp;

    private void Start()
    {
        pickupSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        pickUpAnim = GetComponent<Animator>();
        timer = lifeTime;
        smallSize = transform.localScale * 0.25f;
    }

    private void Update()
    {
        if (!pickUp)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

                if (timer < 2)
                {
                    flashTime = startFlashTime / 4;
                }
                else if (timer < 4)
                {
                    flashTime = startFlashTime / 2;
                }
                else if (timer < 6)
                {
                    if (!flashStarted)
                    {
                        FlashOff();
                        flashStarted = true;
                    }
                    flashTime = startFlashTime;
                }
            }
            else
            {
                effect.transform.parent = ParentManager.instance.effects;
                effect.Stop();
                Destroy(effect, 2);
                Destroy(gameObject);
            }
        }
        else
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            transform.position = Vector3.MoveTowards(transform.position, magnet.position, smoothTime * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, smallSize, smoothTime * Time.deltaTime);
            if (Vector3.Distance(transform.position, magnet.position) < 0.05f)
            {
                if (!isPickedUp)
                {
                    PickedUp();
                    isPickedUp = true;
                }
            }
        }
    }

    private void FlashOff()
    {
        pickupMesh.enabled = false;
        Invoke("FlashOn", flashTime/2);
    }
    private void FlashOn()
    {
        pickupMesh.enabled = true;
        Invoke("FlashOff", flashTime);
    }
    public void PickUp(Transform magnetMuzzle)
    {
        magnet = magnetMuzzle;
        pickUp = true;
        CancelInvoke();
        pickUpAnim.enabled = false;
        pickupMesh.enabled = true;
    }
    public void PickedUp()
    {
        GameManager.instance.magnet.UpdateMetal(value);
        pickupSource.PlayOneShot(metalSounds[Random.Range(0, metalSounds.Length)]);
        pickupMesh.enabled = false;
        Destroy(gameObject, 0.5f);
    }

}
