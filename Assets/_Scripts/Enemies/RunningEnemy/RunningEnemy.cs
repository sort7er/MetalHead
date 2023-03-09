using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;

public class RunningEnemy : MonoBehaviour
{
    [Header("Overall stats")]
    public float turnSmoothTime;

    [Header("IdleState")]
    public Transform[] tempIdleTargets;
    public float idleSpeed;
    public float sightRange;
    public float FOV;
    public float hearingRange;
    public float timeBetweenDistanceCheck;
    public float timeBeforeSus;
    public float minTimeBetweenTargets;
    public float maxTimeBetweenTargets;

    [Header("SusState")]
    public float susSpeed;
    public float minSusDuration;
    public float maxSusDuration;

    [HideInInspector] public Vector3 directionToPlayer;
    [HideInInspector] public Vector3 directionToCamera;
    [HideInInspector] public Vector3 pointOfInterest;
    [HideInInspector] public NavMeshAgent agent;
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
        agent = GetComponent<NavMeshAgent>();
        SwitchState(idleState);
        DistanceCheck();
    }

    private void Update()
    {
        directionToPlayer = GameManager.instance.XROrigin.transform.position + new Vector3(0,1,0) - transform.position;
        directionToCamera = GameManager.instance.cam.transform.position - transform.position;
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
    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }
    private void DistanceCheck()
    {
        enemyDistanceCheck = true;
    }
    public void PointOfInterest(Vector3 position)
    {
        pointOfInterest = position;
    }
    public void WaitBeforeNextTarget()
    {
        Invoke("WaitBeforeNextTarget2", Random.Range(minTimeBetweenTargets, maxTimeBetweenTargets));
    }
    private void WaitBeforeNextTarget2()
    {
        idleState.NextTarget();
    }
    public void SusStart()
    {
        Invoke("SusStart2", 1f);
    }
    private void SusStart2()
    {
        susState.CheckItOut();
    }


    public void EnemySus(Vector3 position)
    {
        if(!playerDetected)
        {
            pointOfInterest = position;
            SwitchState(susState);
        }
    }

}
