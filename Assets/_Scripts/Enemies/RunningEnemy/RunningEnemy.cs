using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RunningEnemy : MonoBehaviour
{
    [Header("Overall stats")]
    public float defaultTurnSmoothTime;
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

    [Header("SearchingState")]
    public float searchingSpeed;
    public float searchingSightRange;
    public float minSearchDuration;
    public float maxSearchDuration;
    public float timeBeforeSeen;
    public Color seenColor;

    [Header("RunState")]
    public float runSpeed;
    public float runSightRange;
    public float rangeBeforeAttack;
    public float timeBeforeLost;

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
    [HideInInspector] public Vector3 movementDircetion;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool enemyDistanceCheck;
    [HideInInspector] public bool playerDetected;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool inView;
    [HideInInspector] public float turnSmoothTime;

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
    private Vector3 thisFrame, lastFrame;
    private float timeBetweenDistanceCheck;



    private void Start()
    {
        SetDistanceCheck(defaultTimeBetweenDistanceCheck);
        SetTurnSpeed(defaultTurnSmoothTime);
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        SwitchState(idleState);
        DistanceCheck();
    }

    private void Update()
    {
        directionToPlayer = GameManager.instance.XROrigin.transform.position + new Vector3(0,0.5f,0) - transform.position;
        directionToCamera = GameManager.instance.cam.transform.position - transform.position;
        currentState.UpdateState(this);
        thisFrame = transform.position;
        movementDircetion = thisFrame - lastFrame;
        lastFrame = transform.position;
    }

    public void SwitchState(EnemyBaseState state)
    {
        CancelInvoke();
        if(!isDead)
        {
            DistanceCheckOff();
            currentState = state;
            SetDistanceCheck(defaultTimeBetweenDistanceCheck);
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
    public void EnemySus(Vector3 position)
    {
        if(!playerDetected)
        {
            SetPointOfInterest(position);
            SwitchState(susState);
        }
    }
    public void EnemyAlert(Vector3 position)
    {

        SetPointOfInterest(position);
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
    public void SetDistanceCheck(float newTime)
    {
        timeBetweenDistanceCheck = newTime;
    }
    public void SetTurnSpeed(float newSpeed)
    {
        turnSmoothTime = newSpeed;
    }
    public void SetPointOfInterest(Vector3 newInterest)
    {
        pointOfInterest = newInterest;
    }
    public void LookingForPlayer(float sightRange)
    {
        if (enemyDistanceCheck)
        {
            if ((GameManager.instance.XROrigin.transform.position - transform.position).magnitude <= sightRange)
            {
                if (Vector3.Angle(directionToPlayer, transform.forward) <= FOV * 0.5f)
                {
                    inView = CheckLineOfSight(false, directionToPlayer);
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

    public bool CheckLineOfSight(bool onlyLower, Vector3 directionToCheck)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToCheck, out hit, 30, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                return true;
            }
            else if (!onlyLower)
            {
                if (Physics.Raycast(transform.position, directionToCamera, out hit, 30, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.gameObject.layer == 7)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }

            }
            else
            { 
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public void DelayedCallback(EnemyBaseState state, string methodName, float time, params object[] parameters)
    {
        StartCoroutine(DelayedCallbackRoutine(state, methodName, time, parameters));
    }

    private IEnumerator DelayedCallbackRoutine(EnemyBaseState state, string methodName, float time, params object[] parameters)
    {

        yield return new WaitForSeconds(time);

        var method = state.GetType().GetMethod(methodName);
        method.Invoke(state, parameters);
    }
    public void RotateToPosition(Vector3 lookAtPoint)
    {
        Quaternion targetAngle = Quaternion.LookRotation(new Vector3(lookAtPoint.x, transform.position.y, lookAtPoint.z) - transform.position, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * turnSmoothTime);
    }
}
