using Unity.XR.CoreUtils;
using UnityEngine;

public class RunningEnemy : MonoBehaviour
{
    [Header("IdleState")]
    public float sightRange;
    public float FOV;
    public float hearingRange;
    public float timeBetweenDistanceCheck;

    [Header("SusState")]
    public float minSusDuration;
    public float maxSusDuration;

    [HideInInspector] public Vector3 directionToPlayer;
    [HideInInspector] public bool enemyDistanceCheck;
    [HideInInspector] public bool playerDetected;

    public EnemyRunState runState = new EnemyRunState();
    public EnemyStunnedState stunnedState = new EnemyStunnedState();
    public EnemyDieState dieState = new EnemyDieState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemySusState susState = new EnemySusState();
    public EnemyCoverState coverState = new EnemyCoverState();
    public EnemyDodgeState dodgeState = new EnemyDodgeState();
    public EnemySearchingState searchingState = new EnemySearchingState();
    public EnemyKickState kickState = new EnemyKickState();

    private EnemyBaseState currentState;




    private void Start()
    {
        SwitchState(idleState);
        DistanceCheck();
    }

    private void Update()
    {
        directionToPlayer = GameManager.instance.XROrigin.transform.position - transform.position;
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    public void DistanceCheckOff()
    {
        enemyDistanceCheck = false;
        Invoke("DistanceCheck", timeBetweenDistanceCheck);
    }
    private void DistanceCheck()
    {
        enemyDistanceCheck = true;
    }
    public void AlertEnemy()
    {
        if(!playerDetected)
        {
            SwitchState(susState);
        }
    }

}
