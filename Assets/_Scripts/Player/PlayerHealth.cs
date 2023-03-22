using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth;
    public float startPitch, endPitch;
    public Animator camAnim;

    private int currentHealth;
    private AudioSource healthSource;
    private float targetVolume;

    void Start()
    {
        currentHealth = startHealth;
        healthSource = GetComponent<AudioSource>();
        HitAudioDone();
    }
    
    public void TakeDamage(int damage)
    {
        if (!GameManager.instance.isDead)
        {
            Debug.Log(damage + " damage given to player");
            currentHealth -= damage;
            HitAudio(currentHealth);
            camAnim.SetTrigger("Hit");
            if (currentHealth < 0)
            {
                Die();
                currentHealth = 0;
            }
        }
    }

    private void Update()
    {
        healthSource.volume = Mathf.Lerp(healthSource.volume, targetVolume, Time.deltaTime * 20);
    }

    private void Die()
    {
        GameManager.instance.IsDead();
        Debug.Log("Player Dead");
    }
    private void HitAudio(int health)
    {


        CancelInvoke();
        targetVolume = 1;
        Invoke(nameof(HitAudioDone), 0.1f);
    }
    private void HitAudioDone()
    {
        targetVolume = 0;
    }

}
