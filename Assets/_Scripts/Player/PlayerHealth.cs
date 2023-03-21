using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth;
    public Animator camAnim;

    private int currentHealth;

    void Start()
    {
        currentHealth = startHealth;
    }
    
    public void TakeDamage(int damage)
    {
        if (!GameManager.instance.isDead)
        {
            Debug.Log(damage + " damage given to player");
            Invoke("ColorShiftDone", 0.2f);
            currentHealth -= damage;
            camAnim.SetTrigger("Hit");
            if (currentHealth < 0)
            {
                Die();
                currentHealth = 0;
            }
        }
    }

    private void Die()
    {
        GameManager.instance.IsDead();
        Debug.Log("Player Dead");
    }

}
