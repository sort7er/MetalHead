using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float fuseTime;
    public float explotionRadius;
    public LayerMask thingsToHit;
    public int[] damageEnemy;
    public int[] damagePlayer;

    private Collider[] colliders = new Collider[100];
    private float timer;
    private bool damageToPlayerGiven;

    private void Start()
    {
        timer = fuseTime;
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Explode();
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
                
                if (Vector3.Distance(colliders[i].transform.position, transform.position) < explotionRadius * 0.33f)
                {
                    damage = damageEnemy[0];
                }
                else if (Vector3.Distance(colliders[i].transform.position, transform.position) < explotionRadius * 0.66f)
                {
                    damage = damageEnemy[1];
                }
                else
                {
                    damage = damageEnemy[2];
                }
                colliders[i].transform.parent.GetComponent<EnemyHealth>().TakeDamage(damage, damage, colliders[i].GetComponent<Rigidbody>(), (colliders[i].transform.position - transform.position) * damage * 35, 6);
                Debug.Log(damage + " given to " + colliders[i].transform.parent.name);
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
                else
                {
                    damage = damagePlayer[2];
                }

                
                
            }
        }
        EffectManager.instance.BombExplotion(transform.position);
        Destroy(gameObject);
    }


}
