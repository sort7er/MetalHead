using Unity.XR.Oculus;
using UnityEngine;

public class VoiceLines : MonoBehaviour
{

    public AudioClip[] idleToSus;
    public AudioClip[] susToIdle;
    public AudioClip[] idleToRun;
    public AudioClip[] runToSearching;
    public AudioClip[] searchingToRun;
    public AudioClip[] hiding;
    public AudioClip[] attacking;

    public AudioClip[] dying;

    private AudioSource voiceLinesSource;
    

    void Start()
    {
        voiceLinesSource = GetComponent<AudioSource>();
    }

    public void IdleSus()
    {
        if (AIManager.instance.canPlayIdleSus)
        {
            int randomNumber = Random.Range(0, idleToSus.Length);
            if (idleToSus.Length > 1)
            {
                while (AIManager.instance.justPlayedIdleSus == randomNumber)
                {
                    randomNumber = Random.Range(0, idleToSus.Length);
                }
            }


            voiceLinesSource.PlayOneShot(idleToSus[randomNumber]);
            AIManager.instance.IdleSus(randomNumber);
            AIManager.instance.IdleSusSound(idleToSus[randomNumber].length);
        }
    }
    public void SusIdle()
    {
        if (AIManager.instance.canPlaySusIdle)
        {
            int randomNumber = Random.Range(0, susToIdle.Length);
            if (susToIdle.Length > 1)
            {
                while (AIManager.instance.justPlayedSusIdle == randomNumber)
                {
                    randomNumber = Random.Range(0, susToIdle.Length);
                }
            }

            voiceLinesSource.PlayOneShot(susToIdle[randomNumber]);
            AIManager.instance.SusIdle(randomNumber);
            AIManager.instance.SusIdleSound(susToIdle[randomNumber].length);
        }
    }

    public void IdleRun()
    {
        if (AIManager.instance.canPlayIdleRun)
        {
            int randomNumber = Random.Range(0, idleToRun.Length);
            if (idleToRun.Length > 1)
            {
                while (AIManager.instance.justPlayedIdleRun == randomNumber)
                {
                    randomNumber = Random.Range(0, idleToRun.Length);
                }
            }

            voiceLinesSource.PlayOneShot(idleToRun[randomNumber]);
            AIManager.instance.IdleRun(randomNumber);
            AIManager.instance.IdleRunSound(idleToRun[randomNumber].length);
        }
    }
    public void RunSearching()
    {
        if (AIManager.instance.canPlayRunSearching)
        {
            int randomNumber = Random.Range(0, runToSearching.Length);
            if (runToSearching.Length > 1)
            {
                while (AIManager.instance.justPlayedRunSearching == randomNumber)
                {
                    randomNumber = Random.Range(0, runToSearching.Length);
                }
            }

            voiceLinesSource.PlayOneShot(runToSearching[randomNumber]);
            AIManager.instance.RunSearching(randomNumber);
            AIManager.instance.RunSearchingSound(runToSearching[randomNumber].length);
        }
    }
    public void SearchingRun()
    {
        if (AIManager.instance.canPlaySearchingRun)
        {
            int randomNumber = Random.Range(0, searchingToRun.Length);
            if (searchingToRun.Length > 1)
            {
                while (AIManager.instance.justPlayedSearchingRun == randomNumber)
                {
                    randomNumber = Random.Range(0, searchingToRun.Length);
                }
            }

            voiceLinesSource.PlayOneShot(searchingToRun[randomNumber]);
            AIManager.instance.SearchingRun(randomNumber);
            AIManager.instance.SearchingRunSound(searchingToRun[randomNumber].length);
        }
    }
    public void Hiding()
    {
        if (AIManager.instance.canPlayHiding)
        {
            int randomNumber = Random.Range(0, hiding.Length);
            if (hiding.Length > 1)
            {
                while (AIManager.instance.justPlayedHiding == randomNumber)
                {
                    randomNumber = Random.Range(0, hiding.Length);
                }
            }

            voiceLinesSource.PlayOneShot(hiding[randomNumber]);
            AIManager.instance.Hiding(randomNumber);
            AIManager.instance.HidingSound(hiding[randomNumber].length);
        }
    }
    public void Attacking()
    {
        if (AIManager.instance.canPlayAttack)
        {
            int randomNumber = Random.Range(0, attacking.Length);
            if (attacking.Length > 1)
            {
                while (AIManager.instance.justPlayedAttacking == randomNumber)
                {
                    randomNumber = Random.Range(0, attacking.Length);
                }
            }

            voiceLinesSource.PlayOneShot(attacking[randomNumber]);
            AIManager.instance.Attacking(randomNumber);
            AIManager.instance.AttackingSound(attacking[randomNumber].length);
        }

    }
    public void Dying()
    {
        if (AIManager.instance.canPlayDying)
        {
            int randomNumber = Random.Range(0, dying.Length);
            if (dying.Length > 1)
            {
                while (AIManager.instance.justPlayedDying == randomNumber)
                {
                    randomNumber = Random.Range(0, dying.Length);
                }
            }

            voiceLinesSource.PlayOneShot(dying[randomNumber]);
            AIManager.instance.Dying(randomNumber);
            AIManager.instance.DyingSound(dying[randomNumber].length);
        }
    }
}
