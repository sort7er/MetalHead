using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;
    [HideInInspector] public RunningEnemy[] runningEnemiesInScene;

    private void Awake()
    {
        instance = this;
        ActualUpdate();
    }

    public float idleDelay;
    public float attackingDelay;

    [HideInInspector] public bool playerIsBeeingAttacked;
    [HideInInspector] public bool talkingOccupied;
    [HideInInspector] public bool idleTalkingOccupied;

    [HideInInspector] public bool canPlayAttack;
    [HideInInspector] public bool canPlayIdle;

    [HideInInspector] public int justPlayedIdle;
    [HideInInspector] public int justPlayedIdleSus;
    [HideInInspector] public int justPlayedSusIdle;
    [HideInInspector] public int justPlayedIdleRun;
    [HideInInspector] public int justPlayedRunSearching;
    [HideInInspector] public int justPlayedSearchingRun;
    [HideInInspector] public int justPlayedHiding;
    [HideInInspector] public int justPlayedAttacking;
    [HideInInspector] public int justPlayedStunned;
    [HideInInspector] public int justPlayedDying;

    private int currentPos;

    private AudioSource idleSource;

    private void Start()
    {
        canPlayAttack = true;
        canPlayIdle = true;
    }

    public void Avoidance(RunningEnemy hitting, RunningEnemy colliding)
    {
        if (colliding.currentState == colliding.idleState)
        {
            if(hitting.currentState == hitting.idleState)
            {
                hitting.HittingAvoidance();
            }
            colliding.RecivingAvoidance();
        }
    }

    public void AvoidanceDone(RunningEnemy hitting, RunningEnemy colliding)
    {
        hitting.AvoidanceDone();
        colliding.AvoidanceDone();
    }

    public bool CheckForAttack()
    {
        if (!playerIsBeeingAttacked)
        {
            playerIsBeeingAttacked = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void DoneAttacking()
    {
        playerIsBeeingAttacked = false;
    }

    public void UpdateArray()
    {
        Invoke(nameof(ActualUpdate), 0.1f);
    }
    private void ActualUpdate()
    {
        runningEnemiesInScene = FindObjectsOfType<RunningEnemy>();
    }
    public void Talking()
    {
        talkingOccupied = true;
        StopIdle();
    }
    public void TalkingDone()
    {
        talkingOccupied = false;
    }
    public void IdleTalking(AudioSource idle)
    {
        idleTalkingOccupied = true;
        idleSource = idle;
    }
    public void StopIdle()
    {
        if(idleSource != null)
        {
            idleSource.Stop();
        }
    }
    public void IdleTalkingDone()
    {
        idleTalkingOccupied = false;
        idleSource = null;
    }
    public void Idle(int justPlayed)
    {
        justPlayedIdle = justPlayed;
    }
    public void IdleSus(int justPlayed)
    {
        justPlayedIdleSus = justPlayed;
    }
    public void SusIdle(int justPlayed)
    {
        justPlayedSusIdle = justPlayed;
    }
    public void IdleRun(int justPlayed)
    {
        justPlayedIdleRun = justPlayed;
    }
    public void RunSearching(int justPlayed)
    {
        justPlayedRunSearching = justPlayed;
    }
    public void SearchingRun(int justPlayed)
    {
        justPlayedSearchingRun = justPlayed;
    }
    public void Hiding(int justPlayed)
    {
        justPlayedHiding = justPlayed;
    }
    public void Attacking(int justPlayed)
    {
        justPlayedAttacking = justPlayed;
    }
    public void Stunned(int justPlayed)
    {
        justPlayedStunned = justPlayed;
    }
    public void Dying(int justPlayed)
    {
        justPlayedDying = justPlayed;
    }

    public void IdleSound(float clipLength)
    {
        canPlayIdle = false;
        Invoke(nameof(IdleSoundDone), clipLength + idleDelay);
    }
    public void IdleSoundDone()
    {
        canPlayIdle = true;
    }
    public void AttackingSound(float clipLength)
    {
        canPlayAttack = false;
        Invoke(nameof(AttackingSoundDone), clipLength + attackingDelay);
    }
    public void AttackingSoundDone()
    {
        canPlayAttack = true;
    }
}
