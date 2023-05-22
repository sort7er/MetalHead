using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float fuseTime;
    public float explotionRadius;
    public LayerMask thingsToHit;

    private Collider[] colliders = new Collider[100];
    public int[] damageAtDifferentRadius;
    private float timer;

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
                Debug.Log("Hit " + colliders[i].transform.parent.name);
            }
            else if( colliders[i].GetComponent<PlayerHealth>() != null)
            {
                Debug.Log("Hit " + colliders[i].name);
            }
        }
        Destroy(gameObject);
    }


}
