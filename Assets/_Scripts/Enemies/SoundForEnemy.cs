using UnityEngine;

public class SoundForEnemy : MonoBehaviour
{
    public AudioClip[] steps;

    public AudioSource enemySource;
    public AudioSource enemySource2;

    private int justPlayed;

    private bool turn;

    public void Step()
    {
        if (turn)
        {
            int randomNumber = Random.Range(0, steps.Length);
            while(justPlayed == randomNumber)
            {
                randomNumber = Random.Range(0, steps.Length);
            }
            
            enemySource.PlayOneShot(steps[randomNumber]);
            justPlayed = randomNumber;
            turn = false;
        }
        else
        {
            int randomNumber = Random.Range(0, steps.Length);
            while (justPlayed == randomNumber)
            {
                randomNumber = Random.Range(0, steps.Length);
            }

            enemySource2.PlayOneShot(steps[randomNumber]);
            justPlayed = randomNumber;
            turn = true;
        }
    }
}
