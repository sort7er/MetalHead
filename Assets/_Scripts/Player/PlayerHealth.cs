using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth;
    public float startPitch, endPitch;
    public Animator camAnim;

    private int currentHealth;
    private AudioSource healthSource;
    private float targetVolume, difference, incerements;

    void Start()
    {
        currentHealth = startHealth;
        healthSource = GetComponent<AudioSource>();
        HitAudioDone();
        difference = startPitch - endPitch;
        incerements = difference / startHealth;
    }
    
    public void TakeDamage(int damage)
    {
        if (!GameManager.instance.isDead)
        {
            currentHealth -= damage;
            camAnim.SetTrigger("Hit");
            if (currentHealth < 0)
            {
                Die();
                currentHealth = 0;
            }
            HitAudio(currentHealth);
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
        healthSource.Play();
        healthSource.pitch = endPitch + incerements * health;
        targetVolume = 1;
        Invoke(nameof(HitAudioDone), 0.1f);
    }
    private void HitAudioDone()
    {
        targetVolume = 0;
    }

}
