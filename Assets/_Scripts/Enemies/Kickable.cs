using UnityEngine;

public class Kickable : MonoBehaviour
{
    [HideInInspector] public bool isBeeingKicked, canDamage;
    public void IsBeeingKicked(bool state)
    {
        isBeeingKicked = state;
        canDamage = state;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
        if (canDamage)
        {
            Debug.Log("1");
            if (collision.transform.GetComponent<PlayerHealth>() != null)
            {
                Debug.Log("2");
                collision.transform.GetComponent<PlayerHealth>().TakeDamage(20);
                EffectManager.instance.SpawnHitPlayerEffect(collision.contacts[0].point);
                canDamage = false;
            }
            if (collision.transform.GetComponent<BodyPart>() != null)
            {
                Debug.Log("3");
                collision.transform.GetComponent<BodyPart>().TakeDamage(20, 20, collision.contacts[0].point - transform.position);
                EffectManager.instance.SpawnBarrelHitEnemy(collision.contacts[0].point);
                canDamage = false;
            }
        }
        IsBeeingKicked(false);
    }
}
