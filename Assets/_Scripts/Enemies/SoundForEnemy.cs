using UnityEngine;

public class SoundForEnemy : MonoBehaviour
{
    public AudioClip[] steps;

    public AudioSource enemySource;
    public AudioSource enemySource2;

    private bool turn;

    public void Step()
    {
        if (turn)
        {
            enemySource.PlayOneShot(steps[Random.Range(0, steps.Length)]);
            turn = false;
        }
        else
        {
            enemySource2.PlayOneShot(steps[Random.Range(0, steps.Length)]);
            turn = true;
        }
    }
}
