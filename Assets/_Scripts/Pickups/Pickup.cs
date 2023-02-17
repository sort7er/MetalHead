using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float lifeTime, startFlashTime;
    public MeshRenderer pickupMesh;
    public ParticleSystem effect;

    private Animator pickUpAnim;
    private float timer;
    private float flashTime;
    private bool flashStarted, pickUp;

    private void Start()
    {
        pickUpAnim = GetComponent<Animator>();
        timer = lifeTime;
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
    public void PickUp()
    {
        pickUp = true;
        CancelInvoke();
        pickUpAnim.enabled = false;
        pickupMesh.enabled = true;
    }

}
