using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public bool randomPitch;
    public AudioClip[] hitSound;


    private AudioSource HitEnemySource;

    private void Start()
    {
        HitEnemySource = GetComponent<AudioSource>();

        if (randomPitch)
        {
            float currentPitch = HitEnemySource.pitch;

            HitEnemySource.pitch = Random.Range(currentPitch - 0.1f, currentPitch + 0.1f);
        }

        HitEnemySource.PlayOneShot(hitSound[Random.Range(0, hitSound.Length)]);
    }
}
