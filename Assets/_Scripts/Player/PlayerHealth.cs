using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth;
    public float startPitch, endPitch;
    public Animator healthVignette;

    private int currentHealth;
    private Watch watch;
    private AudioSource healthSource;
    private float targetVolume, difference, incerements;
    private bool watchFound;

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
            healthVignette.SetTrigger("Hit");
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
                currentHealth = 0;
            }
            if(watch != null)
            {
                watch.UpdateHealth(currentHealth);
            }
            HitAudio(currentHealth);
        }
    }

    private void Update()
    {
        healthSource.volume = Mathf.Lerp(healthSource.volume, targetVolume, Time.deltaTime * 50);
        if(watch == null)
        {
            watch = FindObjectOfType<Watch>();
            watchFound= false;
        }
        else
        {
            if (!watchFound)
            {
                watchFound = true;
                watch.SetNewHealth(currentHealth, startHealth);
            }
        }
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
