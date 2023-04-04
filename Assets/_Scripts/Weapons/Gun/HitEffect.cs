using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public AudioClip[] hitSound;

    private AudioSource HitEnemySource;

    private void Start()
    {
        HitEnemySource = GetComponent<AudioSource>();
        HitEnemySource.PlayOneShot(hitSound[Random.Range(0, hitSound.Length)]);
    }
}
