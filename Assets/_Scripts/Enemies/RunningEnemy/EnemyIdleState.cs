using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBaseState
{
    private Transform enemyTrans;
    private NavMeshAgent agent;
    private Vector3 targetPos, directionToPlayer, directionToCamera, pointOfInterest;
    private Quaternion targetAngle;
    private int currentTarget;
    private bool targetReached, inView;
    private float sightRange, FOV, timer;


    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state idle");
        enemy.SetSpeed(enemy.idleSpeed);
        enemy.sliderBackground.color = enemy.idleDefaultColor;
        enemy.sliderFill.color = enemy.idleDetectionColor;
        enemy.detectionSlider.value = 0f;
        enemy.detectionSlider.maxValue = enemy.timeBeforeSus;
        agent = enemy.agent;
        targetReached = false;
        inView = false;
        sightRange = enemy.idleSightRange;
        FOV = enemy.FOV;
        if(targetPos != null)
        {
            currentTarget = Random.Range(0, enemy.tempIdleTargets.Length);
            targetPos = enemy.tempIdleTargets[currentTarget].position;
            targetAngle = Quaternion.LookRotation(new Vector3(targetPos.x, enemy.transform.position.y, targetPos.z) - enemy.transform.position, enemy.transform.up);
        }
        enemy.agent.SetDestination(targetPos);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        enemyTrans = enemy.transform;
        directionToPlayer = enemy.directionToPlayer;
        directionToCamera = enemy.directionToCamera;
        enemy.detectionSlider.value = timer;

        //Walk to target
        if ((targetPos - enemy.transform.position).magnitude <= 1f && !targetReached)
        {
            if(currentTarget >= enemy.tempIdleTargets.Length - 1)
            {
                currentTarget = 0;
            }
            else
            {
                currentTarget++;
            }
            agent.ResetPath();
            enemy.WaitBeforeNextTarget();
            targetPos = enemy.tempIdleTargets[currentTarget].position;
            targetReached = true;
        }
        else
        {
            enemyTrans.rotation = Quaternion.Slerp(enemyTrans.rotation, targetAngle, Time.deltaTime * enemy.turnSmoothTime);
        }

        //Looking for player
        if (enemy.enemyDistanceCheck)
        {
            if ((GameManager.instance.XROrigin.transform.position - enemyTrans.position).magnitude <= sightRange)
            {
                if (Vector3.Angle(directionToPlayer, enemyTrans.forward) <= FOV * 0.5f)
                {
                    RaycastHit hit;
                    if(Physics.Raycast(enemyTrans.position, directionToPlayer, out hit, 30, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                    {
                        if(hit.transform.gameObject.layer == 7)
                        {
                            inView = true;
                            pointOfInterest = hit.point;
                        }
                        else
                        {
                            if (Physics.Raycast(enemyTrans.position, directionToCamera, out hit, 30, Physics.AllLayers, QueryTriggerInteraction.Ignore))
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
            enemy.DistanceCheckOff();
        }
        if (inView)
        {
            
            if (timer >= enemy.timeBeforeSus)
            {
                enemy.pointOfInterest = pointOfInterest;
                timer = enemy.timeBeforeSus;
                enemy.SwitchState(enemy.susState);
                
            }
            else
            {
                timer += Time.deltaTime;
            }

        }
        else
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
        }

    }
    public void NextTarget()
    {
        targetAngle = Quaternion.LookRotation(new Vector3(targetPos.x, enemyTrans.position.y, targetPos.z) - enemyTrans.position, enemyTrans.up);
        agent.SetDestination(targetPos);
        targetReached = false;
    }
}
    