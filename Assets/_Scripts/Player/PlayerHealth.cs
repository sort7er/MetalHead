using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth;
    public float startPitch, endPitch;
    public Animator healthVignette;
    public AudioSource heartbeatSource;
    [Range(0,1)]
    public float hapticIntensity;
    public float duration;
    public int[] healthAtUpgrade;


    private int currentHealth;
    private int currentHealthLevel;
    private Watch watch;
    private AudioSource healthSource;
    private float targetVolume, difference, incerements;
    private bool watchFound, heartbeatGiven, heartbeatDone;
    private XRDirectInteractor leftInteractor, rightInteractor;

    void Start()
    {
        currentHealth = startHealth;
        healthSource = GetComponent<AudioSource>();
        HitAudioDone();
        difference = startPitch - endPitch;
        incerements = difference / startHealth;
        leftInteractor = GameManager.instance.leftHand.GetComponent<XRDirectInteractor>();
        rightInteractor = GameManager.instance.rightHand.GetComponent<XRDirectInteractor>();
        heartbeatDone = true;
    }

    public void TakeDamage(int damage)
    {
        if (!GameManager.instance.isDead)
        {
            
            currentHealth -= damage;

            if (currentHealth < startHealth * 0.2f && !heartbeatGiven)
            {
                healthVignette.Play("LowHealth");
                heartbeatSource.Play();
                heartbeatGiven = true;
                heartbeatDone = false;
                Invoke(nameof(HeartbeatDone), 2);
            }
            else if (heartbeatDone)
            {
                HitAudio(currentHealth);
                healthVignette.Play("Hit");
            }


            if (currentHealth <= 0)
            {
                leftInteractor.SendHapticImpulse(hapticIntensity*2, duration*2);
                rightInteractor.SendHapticImpulse(hapticIntensity*2, duration*2);
                Die();
                currentHealth = 0;
            }
            if(watch != null)
            {
                watch.UpdateHealth(currentHealth);
                leftInteractor.SendHapticImpulse(hapticIntensity, duration);
                rightInteractor.SendHapticImpulse(hapticIntensity, duration);
            }
        }
    }
    private void HeartbeatDone()
    {
        heartbeatDone = true;
    }

    public void UpgradeHealth()
    {
        if(currentHealthLevel < healthAtUpgrade.Length)
        {
            currentHealth = healthAtUpgrade[currentHealthLevel];
            startHealth = healthAtUpgrade[currentHealthLevel];
            difference = startPitch - endPitch;
            incerements = difference / startHealth;
            watch.SetNewHealth(currentHealth, startHealth);
            watch.HealthRing(currentHealthLevel);
            EffectManager.instance.PickUpRingEffect(watch.healthRing.transform.position);
            healthVignette.Play("UpgradeHealth");
            currentHealthLevel++;
            heartbeatGiven = false;
            leftInteractor.SendHapticImpulse(hapticIntensity * 2, duration * 2);
            rightInteractor.SendHapticImpulse(hapticIntensity * 2, duration * 2);
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
        healthVignette.Play("Die");
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
