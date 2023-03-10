using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RunningEnemy : MonoBehaviour
{
    [Header("Overall stats")]
    public float turnSmoothTime;
    public float defaultTimeBetweenDistanceCheck;

    [Header("IdleState")]
    public float idleSpeed;
    public float idleSightRange;
    public float FOV;
    public float timeBeforeSus;
    public float minTimeBetweenTargets;
    public float maxTimeBetweenTargets;
    public Color idleDefaultColor;
    public Color idleDetectionColor;

    [Header("SusState")]
    public float susSpeed;
    public float timeBeforeDetect;
    public float susSightRange;
    public float minSusDuration;
    public float maxSusDuration;
    public Color susDetectionColor;

    [Header("DieState")]
    public float timeDead;

    [Header("References")]
    public Slider detectionSlider;
    public Image sliderBackground;
    public Image sliderFill;
    public Transform[] tempIdleTargets;

    [HideInInspector] public Vector3 directionToPlayer;
    [HideInInspector] public Vector3 directionToCamera;
    [HideInInspector] public Vector3 pointOfInterest;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool enemyDistanceCheck;
    [HideInInspector] public bool playerDetected;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool inView;

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
    private float timeBetweenDistanceCheck;



    private void Start()
    {
        SetDistanceCheck(defaultTimeBetweenDistanceCheck);
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        SwitchState(idleState);
        DistanceCheck();
    }

    private void Update()
    {
        Debug.Log(inView);
        directionToPlayer = GameManager.instance.XROrigin.transform.position + new Vector3(0,1,0) - transform.position;
        directionToCamera = GameManager.instance.cam.transform.position - transform.position;
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        CancelInvoke();
        if(!isDead)
        {
            inView = false;
            DistanceCheckOff();
            currentState = state;
            state.EnterState(this);
        }
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
        Invoke("SusStart2", 2f);
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
    public void EnemyAlert(Vector3 position)
    {

        pointOfInterest = position;
        SwitchState(searchingState);
        PlayerDetected();
    }
    public void PlayerDetected()
    {
        if (!playerDetected)
        {
            playerDetected = true;
        }
    }
    public void Die()
    {
        SwitchState(dieState);
        Destroy(gameObject, timeDead);
        isDead = true;
    }
    public void LookingForPlayer(float sightRange)
    {
        //Looking for player
        if (enemyDistanceCheck)
        {
            if ((GameManager.instance.XROrigin.transform.position - transform.position).magnitude <= sightRange)
            {
                if (Vector3.Angle(directionToPlayer, transform.forward) <= FOV * 0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, 30, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                    {
                        if (hit.transform.gameObject.layer == 7)
                        {
                            inView = true;
                            pointOfInterest = hit.point;
                        }
                        else
                        {
                            if (Physics.Raycast(transform.position, directionToCamera, out hit, 30, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                            {
                                if (hit.transform.gameObject.layer == 7)
                                {
                                    inView = true;
                                    pointOfInterest = hit.point;
                                }

                            }

                        }
                    }
                }
                else
                {
                    inView = false;
                }
            }
            else
            {
                inView = false;
            }
            DistanceCheckOff();
        }
    }
    public void SetDistanceCheck(float newTime)
    {
        timeBetweenDistanceCheck = newTime;
    }
}
