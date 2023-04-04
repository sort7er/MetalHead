using Unity.XR.Oculus;
using UnityEngine;

public class VoiceLines : MonoBehaviour
{

    public AudioClip[] idle;
    public AudioClip[] idleToSus;
    public AudioClip[] susToIdle;
    public AudioClip[] idleToRun;
    public AudioClip[] runToSearching;
    public AudioClip[] searchingToRun;
    public AudioClip[] hiding;
    public AudioClip[] attacking;
    public AudioClip[] stunned;
    public AudioClip[] dying;

    private AudioSource voiceLinesSource;
    private bool canTalk, canTalkIdle;
    

    void Start()
    {
        voiceLinesSource = GetComponent<AudioSource>();
    }
    public void Idle()
    {
        if (!AIManager.instance.idleTalkingOccupied && !AIManager.instance.talkingOccupied)
        {
            canTalkIdle = true;
            AIManager.instance.IdleTalking(voiceLinesSource);
        }
        if (AIManager.instance.canPlayIdle && !canTalk && canTalkIdle)
        {
            int randomNumber = Random.Range(0, idle.Length);
            if (idle.Length > 1)
            {
                while (AIManager.instance.justPlayedIdle == randomNumber)
                {
                    randomNumber = Random.Range(0, idle.Length);
                }
            }
            CancelInvoke();
            Invoke(nameof(TalkingDone), idle[randomNumber].length);
            voiceLinesSource.clip = idle[randomNumber];
            voiceLinesSource.Play();
            AIManager.instance.Idle(randomNumber);
            AIManager.instance.IdleSound(idle[randomNumber].length);
        }

    }
    public void IdleSus()
    {
        if(!AIManager.instance.talkingOccupied)
        {
            canTalk = true;
            AIManager.instance.Talking();
        }

        if (canTalk)
        {
            int randomNumber = Random.Range(0, idleToSus.Length);
            if (idleToSus.Length > 1)
            {
                while (AIManager.instance.justPlayedIdleSus == randomNumber)
                {
                    randomNumber = Random.Range(0, idleToSus.Length);
                }
            }


            voiceLinesSource.clip = idleToSus[randomNumber];
            voiceLinesSource.Play();
            CancelInvoke();
            Invoke(nameof(TalkingDone), idleToSus[randomNumber].length);

            AIManager.instance.IdleSus(randomNumber);
        }
    }
    public void SusIdle()
    {
        if (!AIManager.instance.talkingOccupied)
        {
            canTalk = true;
            AIManager.instance.Talking();
        }
        if (canTalk)
        {
            int randomNumber = Random.Range(0, susToIdle.Length);
            if (susToIdle.Length > 1)
            {
                while (AIManager.instance.justPlayedSusIdle == randomNumber)
                {
                    randomNumber = Random.Range(0, susToIdle.Length);
                }
            }
            CancelInvoke();
            Invoke(nameof(TalkingDone), susToIdle[randomNumber].length);
            voiceLinesSource.clip = susToIdle[randomNumber];
            voiceLinesSource.Play();
            AIManager.instance.SusIdle(randomNumber);
        }
    }

    public void IdleRun()
    {
        if (!AIManager.instance.talkingOccupied)
        {
            canTalk = true;
            AIManager.instance.Talking();
        }
        if (canTalk)
        {
            int randomNumber = Random.Range(0, idleToRun.Length);
            if (idleToRun.Length > 1)
            {
                while (AIManager.instance.justPlayedIdleRun == randomNumber)
                {
                    randomNumber = Random.Range(0, idleToRun.Length);
                }
            }
            CancelInvoke();
            Invoke(nameof(TalkingDone), idleToRun[randomNumber].length);
            voiceLinesSource.clip = idleToRun[randomNumber];
            voiceLinesSource.Play();
            AIManager.instance.IdleRun(randomNumber);
        }
    }
    public void RunSearching()
    {
        if (!AIManager.instance.talkingOccupied)
        {
            canTalk = true;
            AIManager.instance.Talking();
        }
        if (canTalk)
        {
            int randomNumber = Random.Range(0, runToSearching.Length);
            if (runToSearching.Length > 1)
            {
                while (AIManager.instance.justPlayedRunSearching == randomNumber)
                {
                    randomNumber = Random.Range(0, runToSearching.Length);
                }
            }
            CancelInvoke();
            Invoke(nameof(TalkingDone), runToSearching[randomNumber].length);
            voiceLinesSource.clip = runToSearching[randomNumber];
            voiceLinesSource.Play();
            AIManager.instance.RunSearching(randomNumber);
        }
    }
    public void SearchingRun()
    {
        if (!AIManager.instance.talkingOccupied)
        {
            canTalk = true;
            AIManager.instance.Talking();
        }
        if (canTalk)
        {
            int randomNumber = Random.Range(0, searchingToRun.Length);
            if (searchingToRun.Length > 1)
            {
                while (AIManager.instance.justPlayedSearchingRun == randomNumber)
                {
                    randomNumber = Random.Range(0, searchingToRun.Length);
                }
            }
            CancelInvoke();
            Invoke(nameof(TalkingDone), searchingToRun[randomNumber].length);
            voiceLinesSource.clip = searchingToRun[randomNumber];
            voiceLinesSource.Play();
            AIManager.instance.SearchingRun(randomNumber);
        }
    }
    public void Hiding()
    {
        if (!AIManager.instance.talkingOccupied)
        {
            canTalk = true;
            AIManager.instance.Talking();
        }
        if (canTalk)
        {
            int randomNumber = Random.Range(0, hiding.Length);
            if (hiding.Length > 1)
            {
                while (AIManager.instance.justPlayedHiding == randomNumber)
                {
                    randomNumber = Random.Range(0, hiding.Length);
                }
            }
            CancelInvoke();
            Invoke(nameof(TalkingDone), hiding[randomNumber].length);
            voiceLinesSource.clip = hiding[randomNumber];
            voiceLinesSource.Play();
            AIManager.instance.Hiding(randomNumber);
        }
    }
    public void Attacking()
    {
        if (!AIManager.instance.talkingOccupied)
        {
            canTalk = true;
            AIManager.instance.Talking();
        }
        if (AIManager.instance.canPlayAttack && canTalk)
        {
            int randomNumber = Random.Range(0, attacking.Length);
            if (attacking.Length > 1)
            {
                while (AIManager.instance.justPlayedAttacking == randomNumber)
                {
                    randomNumber = Random.Range(0, attacking.Length);
                }
            }
            CancelInvoke();
            Invoke(nameof(TalkingDone), attacking[randomNumber].length);
            voiceLinesSource.clip = attacking[randomNumber];
            voiceLinesSource.Play();
            AIManager.instance.Attacking(randomNumber);
            AIManager.instance.AttackingSound(attacking[randomNumber].length);
        }

    }

    public void Stunned()
    {
        if (!AIManager.instance.talkingOccupied)
        {
            canTalk = true;
            AIManager.instance.Talking();
        }
        if (canTalk)
        {
            int randomNumber = Random.Range(0, stunned.Length);
            if (stunned.Length > 1)
            {
                while (AIManager.instance.justPlayedStunned == randomNumber)
                {
                    randomNumber = Random.Range(0, stunned.Length);
                }
            }
            CancelInvoke();
            Invoke(nameof(TalkingDone), stunned[randomNumber].length);
            voiceLinesSource.clip = stunned[randomNumber];
            voiceLinesSource.Play();
            AIManager.instance.Stunned(randomNumber);
        }

    }

    public void Dying()
    {
        if (!AIManager.instance.talkingOccupied)
        {
            AIManager.instance.Talking();
            canTalk = true;
        }
        if (canTalk)
        {
            int randomNumber = Random.Range(0, dying.Length);
            if (dying.Length > 1)
            {
                while (AIManager.instance.justPlayedDying == randomNumber)
                {
                    randomNumber = Random.Range(0, dying.Length);
                }
            }
            CancelInvoke();
            Invoke(nameof(TalkingDone), dying[randomNumber].length);
            voiceLinesSource.clip = dying[randomNumber];
            voiceLinesSource.Play();
            AIManager.instance.Dying(randomNumber);
        }
    }
    private void TalkingDone()
    {
        canTalk = false;
        canTalkIdle = false;
        AIManager.instance.TalkingDone();
        AIManager.instance.IdleTalkingDone();
    }
}
