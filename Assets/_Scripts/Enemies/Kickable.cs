using UnityEngine;

public class Kickable : MonoBehaviour
{
    [HideInInspector] public bool isBeeingKicked, canDamage;


    private void OnCollisionEnter(Collision collision)
    {
        if (canDamage)
        {
            if (collision.transform.GetComponent<PlayerHealth>() != null)
            {
                collision.transform.GetComponent<PlayerHealth>().TakeDamage(20);
                EffectManager.instance.SpawnHitPlayerEffect(collision.contacts[0].point);
                CannotDamage();
            }
            if (collision.transform.GetComponent<BodyPart>() != null)
            {
                collision.transform.GetComponent<BodyPart>().TakeDamage(20, 60, collision.contacts[0].point - transform.position);
                EffectManager.instance.SpawnBarrelHitEnemy(collision.contacts[0].point);
                CannotDamage();
            }
            if(collision.gameObject.layer == 6 || collision.gameObject.layer == 10)
            {
                CannotDamage();
            }
        }
    }
    public void IsBeeingKicked(bool state)
    {
        isBeeingKicked = state;
    }
    public void CanDamage()
    {
        CancelInvoke();
        canDamage = true;
        Invoke(nameof(CannotDamage), 3f);
    }
    private void CannotDamage()
    {
        canDamage = false;
    }
}
