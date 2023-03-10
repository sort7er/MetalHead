using UnityEngine;
using UnityEngine.AI;

public class EnemySusState : EnemyBaseState
{
    private Transform enemyTrans;
    private NavMeshAgent agent;
    private Vector3 directionToPlayer, directionToCamera, pointOfInterest;
    private Quaternion targetAngle;
    private bool targetReached, inView;
    private float sightRange, FOV, timer;

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state sus");
        agent = enemy.agent;
        enemy.SetSpeed(enemy.susSpeed);
        enemy.sliderBackground.color = enemy.idleDetectionColor;
        enemy.sliderFill.color = enemy.susDetectionColor;
        enemy.detectionSlider.value = 0;
        pointOfInterest = enemy.pointOfInterest;
        targetReached = false;
        inView = false;
        sightRange = enemy.susSightRange;
        FOV = enemy.FOV;
        agent.ResetPath();
        enemy.SusStart();
        
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        enemyTrans = enemy.transform;
        directionToPlayer = enemy.directionToPlayer;
        directionToCamera = enemy.directionToCamera;
        enemy.detectionSlider.value = timer;

        //Walk to target
        if ((pointOfInterest - enemy.transform.position).magnitude <= 1f && !targetReached)
        {
            agent.ResetPath();
            targetReached = true;
            Debug.Log("Yeah");
        }
        else
        {
            targetAngle = Quaternion.LookRotation(new Vector3(enemy.pointOfInterest.x, enemy.transform.position.y, enemy.pointOfInterest.z) - enemy.transform.position, enemy.transform.up);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetAngle, Time.deltaTime * enemy.turnSmoothTime);
        }

        //Looking for player
        if (enemy.enemyDistanceCheck)
        {
            if ((GameManager.instance.XROrigin.transform.position - enemyTrans.position).magnitude <= sightRange)
            {
                if (Vector3.Angle(directionToPlayer, enemyTrans.forward) <= FOV * 0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(enemyTrans.position, directionToPlayer, out hit, 30, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                    {
                        if (hit.transform.gameObject.layer == 7)
                        {
                            inView = true;
                        }
                        else
                        {
                            if (Physics.Raycast(enemyTrans.position, directionToCamera, out hit, 30, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                            {
                                if (hit.transform.gameObject.layer == 7)
                                {
                                    inView = true;
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
            agent.ResetPath();

            if (timer >= enemy.timeBeforeDetect)
            {
                timer = enemy.timeBeforeDetect;
                enemy.SwitchState(enemy.runState);

            }
            else
            {
                timer += Time.deltaTime;
            }

        }
        else
        {
            CheckItOut();
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
        }

    }
    public void CheckItOut()
    {
        agent.SetDestination(pointOfInterest);
    }
}
