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

    [HideInInspector] public bool canPlayIdleSus;
    [HideInInspector] public bool canPlaySusIdle;
    [HideInInspector] public bool canPlayIdleRun;
    [HideInInspector] public bool canPlayRunSearching;
    [HideInInspector] public bool canPlaySearchingRun;
    [HideInInspector] public bool canPlayHiding;
    [HideInInspector] public bool canPlayAttack;
    [HideInInspector] public bool canPlayDying;

    [HideInInspector] public int justPlayedIdleSus;
    [HideInInspector] public int justPlayedSusIdle;
    [HideInInspector] public int justPlayedIdleRun;
    [HideInInspector] public int justPlayedRunSearching;
    [HideInInspector] public int justPlayedSearchingRun;
    [HideInInspector] public int justPlayedHiding;
    [HideInInspector] public int justPlayedAttacking;
    [HideInInspector] public int justPlayedDying;

    private int currentPos;

    private void Start()
    {
        canPlayIdleSus = true;
        canPlaySusIdle = true;
        canPlayIdleRun= true;
        canPlayRunSearching= true;
        canPlaySearchingRun= true;
        canPlayHiding= true;
        canPlayAttack= true;
        canPlayDying= true;
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
    public void Dying(int justPlayed)
    {
        justPlayedDying = justPlayed;
    }
    public void IdleSusSound(float clipLength)
    {
        canPlayIdleSus = false;
        Invoke(nameof(IdleSusSoundDone), clipLength);
    }
    public void IdleSusSoundDone()
    {
        canPlayIdleSus = true;
    }
    public void SusIdleSound(float clipLength)
    {
        canPlaySusIdle = false;
        Invoke(nameof(SusIdleSoundDone), clipLength);
    }
    public void SusIdleSoundDone()
    {
        canPlaySusIdle = true;
    }
    public void IdleRunSound(float clipLength)
    {
        canPlayIdleRun = false;
        Invoke(nameof(IdleRunSoundDone), clipLength);
    }
    public void IdleRunSoundDone()
    {
        canPlayIdleRun = true;
    }
    public void RunSearchingSound(float clipLength)
    {
        canPlayRunSearching = false;
        Invoke(nameof(RunSearchingSoundDone), clipLength);
    }
    public void RunSearchingSoundDone()
    {
        canPlayRunSearching = true;
    }
    public void SearchingRunSound(float clipLength)
    {
        canPlaySearchingRun = false;
        Invoke(nameof(SearchingRunSoundDone), clipLength);
    }
    public void SearchingRunSoundDone()
    {
        canPlaySearchingRun = true;
    }
    public void HidingSound(float clipLength)
    {
        canPlayHiding = false;
        Invoke(nameof(HidingSoundDone), clipLength);
    }
    public void HidingSoundDone()
    {
        canPlayHiding = true;
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
    public void DyingSound(float clipLength)
    {
        canPlayDying = false;
        Invoke(nameof(DyingSoundDone), clipLength);
    }
    public void DyingSoundDone()
    {
        canPlayDying = true;
    }
}
