using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RunningEnemy : MonoBehaviour
{
    [Header("Overall stats")]
    public float defaultTurnSmoothTime;
    public float defaultTimeBetweenDistanceCheck;
    public float defaultFOV;
    public float distanceBeforeImmediateDetection;

    [Header("IdleState")]
    public float idleSpeed;
    public float idleSightRange;
    public float timeBeforeSus;
    public float minTimeBetweenTargets;
    public float maxTimeBetweenTargets;
    public Color idleColor;

    [Header("SusState")]
    public float susSpeed;
    public float timeBeforeDetect;
    public float susSightRange;
    public float minSusDuration;
    public float maxSusDuration;
    public Color susColor;

    [Header("SearchingState")]
    public float searchingSpeed;
    public float searchingSightRange;
    public float minSearchDuration;
    public float maxSearchDuration;
    public float timeBeforeSeen;
    public Color searchingColor;

    [Header("RunState")]
    public float runSpeed;
    public float runSightRange;
    public float rangeBeforeAttack;
    public float timeBeforeLost;
    public Color detectedColor;

    [Header("CoverState")]
    public float minCoverDuration;
    public float maxCoverDuration;
    public float coverSpeed;
    public float dodgeSpeed;
    public float sightRangeForCover;
    public float checkCoverRadius;
    public float minPlayerDistance;
    public float defaultTimeCoverCheck;
    [Range(-1,1)]
    public float hideSensitivity;
    public LayerMask hidebleLayer;

    [Header("DieState")]
    public float timeDead;

    [Header("References")]
    public Transform enemyModel;
    public Transform headTrans;
    public Transform[] tempIdleTargets;
    public Rigidbody[] limbs;
    public MeshRenderer[] glowingParts; 

    [HideInInspector] public Vector3 directionToPlayer;
    [HideInInspector] public Vector3 directionToCamera;
    [HideInInspector] public Vector3 pointOfInterest;
    [HideInInspector] public Vector3 directionToPointOfInterest;
    [HideInInspector] public Vector3 movementDircetion;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public bool enemyDistanceCheck;
    [HideInInspector] public bool playerDetected;
    [HideInInspector] public bool playerInSight;
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

    private Animator enemyAnim;
    private EnemyBaseState currentState;
    private Vector3 thisFrame, lastFrame;
    private float timeBetweenDistanceCheck, FOV;



    private void Start()
    {
        SetDistanceCheck(defaultTimeBetweenDistanceCheck);
        SetTurnSpeed(defaultTurnSmoothTime);
        enemyAnim = enemyModel.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        FOV = defaultFOV;
        DistanceCheck();
        EnableRagdoll(false);
        SwitchState(idleState);

    }

    private void Update()
    {
        directionToPlayer = GameManager.instance.XROrigin.transform.position - headTrans.position;
        directionToCamera = GameManager.instance.cam.transform.position - headTrans.position;
        directionToPointOfInterest = pointOfInterest - headTrans.position;
        currentState.UpdateState(this);
        thisFrame = transform.position;
        movementDircetion = thisFrame - lastFrame;
        lastFrame = transform.position;
    }

    public void SwitchState(EnemyBaseState state)
    {
        StopAllCoroutines();
        if(!isDead)
        {
            DistanceCheckOff();
            currentState = state;
            SetDistanceCheck(defaultTimeBetweenDistanceCheck);
            SetTurnSpeed(defaultTurnSmoothTime);
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
        if (!playerInSight)
        {
            SetPointOfInterest(position);
            SwitchState(searchingState);
            PlayerDetected();
        }
    }
    public void PlayerInSight(bool state)
    {
        playerInSight = state;
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
        Destroy(enemyModel.gameObject, timeDead);
        isDead = true;
    }
    public void AddForce(Rigidbody rb, Vector3 damageDir)
    {
        rb.AddForce(damageDir, ForceMode.Impulse);
    }
    public void Hide()
    {
        if(playerDetected)
        {
            SwitchState(coverState);
        }
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
    public void SetGlowColor(Color newColor)
    {
        for(int i = 0; i < glowingParts.Length; i++)
        {
            glowingParts[i].material.color = newColor;
            glowingParts[i].material.SetColor("_EmissionColor", newColor);
        }
    }
    public void EnableRagdoll(bool state)
    {
        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].isKinematic = !state;
        }
        enemyAnim.enabled = !state;
        if (state)
        {
            enemyModel.parent = ParentManager.instance.enemies;
        }
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
        if (Physics.Raycast(headTrans.position, directionToCheck, out hit, 30, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                return true;
            }
            else if (!onlyLower)
            {
                if (Physics.Raycast(headTrans.position, directionToCamera, out hit, 30, Physics.AllLayers, QueryTriggerInteraction.Ignore))
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(headTrans.position, directionToPlayer * 5);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(headTrans.position, directionToCamera * 5);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(headTrans.position, directionToPointOfInterest * 5);

    }
}
