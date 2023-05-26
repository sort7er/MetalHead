using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bomb : MonoBehaviour
{
    [Header("Values")]
    public float fuseTime;
    public float explotionRadius;
    public float bombCoolDown;
    public int[] damageEnemy;
    public int[] damagePlayer;

    [Header("References")]
    public LayerMask thingsToHit;
    public Transform leftAttach;
    public Transform rightAttach;
    public AudioClip[] bombTicks;
    public MeshRenderer bombGlow;

    private AudioSource bombSource;
    private Rigidbody rb;
    private XRGrabInteractable xrGrabInteractable;
    private Collider[] colliders = new Collider[50];
    private float timer, currentInterval;
    private bool damageToPlayerGiven, tick, hitFloor;
    private float startInterval;

    private void Start()
    {
        startInterval = 0.8f;
        rb = GetComponent<Rigidbody>();
        bombSource = GetComponent<AudioSource>();
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        timer = fuseTime;
        currentInterval = startInterval;
        PlaySound();
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            currentInterval -= Time.deltaTime / (fuseTime * 1.3f);
        }
        else
        {
            Explode();
        }
    }

    private void PlaySound()
    {
        if (!tick)
        {
            bombSource.clip = bombTicks[1];
            tick = true;
        }
        else
        {
            bombSource.clip = bombTicks[0];
            tick = false;
        }

        bombGlow.material.EnableKeyword("_EMISSION");
        bombSource.Play();
        Invoke(nameof(TurnOffGlow), currentInterval * 0.2f);
        Invoke(nameof(PlaySound), currentInterval);
    }
    private void TurnOffGlow()
    {
        bombGlow.material.DisableKeyword("_EMISSION");
    }

    public void GrabBomb()
    {
        if (GameManager.instance.CheckGameObject(gameObject) == 1)
        {
            xrGrabInteractable.attachTransform = leftAttach;
        }
        if (GameManager.instance.CheckGameObject(gameObject) == 2)
        {
            xrGrabInteractable.attachTransform = rightAttach;
        }
        hitFloor = false;
        gameObject.layer = 8;
    }
    public void ReleaseBomb()
    {
        Invoke(nameof(ChangeLayer), 0.2f);
    }
    private void ChangeLayer()
    {
        gameObject.layer = 12;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6 && !hitFloor)
        {
            rb.velocity = rb.velocity * 0.1f; rb.angularVelocity = rb.angularVelocity * 0.1f;
            hitFloor = true;
        }
    }

    public void Explode()
    {
        int hits = Physics.OverlapSphereNonAlloc(transform.position, explotionRadius, colliders, thingsToHit);

        for (int i = 0; i < hits; i++)
        {
            if (colliders[i].GetComponent<SoundForEnemy>() != null)
            {
                int damage;
                
                if (Vector3.Distance(colliders[i].transform.position, transform.position) < explotionRadius * 0.17f)
                {
                    damage = damageEnemy[0];
                }
                else if (Vector3.Distance(colliders[i].transform.position, transform.position) < explotionRadius * 0.5f)
                {
                    damage = damageEnemy[1];
                }
                else
                {
                    damage = damageEnemy[2];
                }
                if (colliders[i].transform.parent.GetComponent<EnemyHealth>() != null)
                {
                    colliders[i].transform.parent.GetComponent<EnemyHealth>().TakeDamage(damage, damage * 2, colliders[i].GetComponent<Rigidbody>(), (colliders[i].transform.position - transform.position) * damage * 10, 6);
                }
            }
            else if( colliders[i].GetComponent<PlayerHealth>() != null && !damageToPlayerGiven)
            {
                int damage;
                
                if (Vector3.Distance(colliders[i].transform.position, transform.position) < explotionRadius * 0.25f)
                {
                    damage = damagePlayer[0];
                    colliders[i].GetComponent<PlayerHealth>().TakeDamage(damage);
                    damageToPlayerGiven = true;
                }
                else if (Vector3.Distance(colliders[i].transform.position, transform.position) < explotionRadius * 0.5f)
                {
                    damage = damagePlayer[1];
                    colliders[i].GetComponent<PlayerHealth>().TakeDamage(damage);
                    damageToPlayerGiven = true;
                }           
            }
            else if(colliders[i].GetComponent<Rigidbody>() != null && !colliders[i].GetComponent<Rigidbody>().isKinematic && colliders[i].GetComponent<XRGrabInteractable>() != null && colliders[i].transform != transform)
            {
                colliders[i].GetComponent<Rigidbody>().AddExplosionForce(100000 / Vector3.Distance(colliders[i].transform.position, transform.position), transform.position, explotionRadius);
            }
        }
        EffectManager.instance.BombExplotion(transform.position);
        AIManager.instance.BombDone();
        Destroy(gameObject);
    }
}
