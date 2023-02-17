using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float lifeTime, startFlashTime;
    public MeshRenderer pickup;
    public ParticleSystem effect;

    private float timer;
    private float flashTime;
    private bool flashStarted;

    private void Start()
    {
        timer = lifeTime;
    }

    private void Update()
    {
        if(timer > 0)
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
            else if(timer < 6)
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

    private void FlashOff()
    {
        pickup.enabled = false;
        Invoke("FlashOn", flashTime/2);
    }
    private void FlashOn()
    {
        pickup.enabled = true;
        Invoke("FlashOff", flashTime);
    }

}
