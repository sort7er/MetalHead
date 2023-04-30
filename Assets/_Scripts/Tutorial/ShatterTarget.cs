using UnityEngine;

public class ShatterTarget : MonoBehaviour
{

    public Rigidbody[] shards;
    public float force, radius;

    void Start()
    {
        for(int i = 0; i < shards.Length; i++)
        {
            shards[i].AddExplosionForce(force, transform.position, radius);
        }
    }
}
