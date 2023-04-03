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

    public float attackingDelay;

    [HideInInspector] public bool playerIsBeeingAttacked;
    [HideInInspector] public bool talkingOccupied;

    [HideInInspector] public bool canPlayAttack = true;

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
    }
    public void TalkingDone()
    {
        talkingOccupied = false;
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
