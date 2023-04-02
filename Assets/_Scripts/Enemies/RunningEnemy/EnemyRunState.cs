using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class EnemyRunState : EnemyBaseState
{
    private float timer;
    private bool scanning, distanceSet, lineOfSightToPlayer;
    private NavMeshAgent agent;
    private Animator enemyAnim;
    private Kickable currentKickable;
    private Collider[] colliders = new Collider[3];
    private RunningEnemy runningEnemy;
    private float randomDistance;

    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state run");
        runningEnemy = enemy;
        agent = enemy.agent;
        enemyAnim = enemy.enemyAnim;
        enemyAnim.SetBool("IsMoving", true);
        enemy.SetAnimSpeed(0.75f);
        enemy.agent.ResetPath();
        enemy.SetGlowColor(enemy.detectedColor);
        enemy.SetSpeed(enemy.runSpeed);
        enemy.PlayerInSight(true);
        enemy.PlayerDetected();
        timer = 0;
        scanning = false;
        currentKickable = null;
        enemy.rig.SetRig(true);
        distanceSet = false;
        randomDistance = Random.Range(enemy.rangeBeforeAttack + 1, enemy.rangeBeforeAttack + 5);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        RaycastHit hit;
        if(Physics.Raycast(enemy.transform.position, (GameManager.instance.XROrigin.transform.position - enemy.transform.position), out hit, enemy.rangeBeforeAttack + 0.5f, runningEnemy.hidebleLayer))
        {
            if(hit.transform.tag == "Player")
            {
                lineOfSightToPlayer = true;
            }
            else
            {
                lineOfSightToPlayer = false;
            }
        }
        else
        {
            lineOfSightToPlayer = false;
        }
        if ((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= enemy.rangeBeforeAttack && lineOfSightToPlayer)
        {
            enemy.SwitchState(enemy.attackState);
            enemy.agent.ResetPath();
        }
        else
        {
            enemy.rig.SetTarget(GameManager.instance.cam.transform.position);

            if (CheckForEnemies() && (GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude < enemy.rangeBeforeAttack + 5)
            {
                enemy.agent.ResetPath();
                enemyAnim.SetBool("IsMoving", false);
            }
            else
            {
                enemyAnim.SetBool("IsMoving", true);
                enemy.agent.SetDestination(GameManager.instance.cam.transform.position);
            }

            if ((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= randomDistance)
            {
                if (!distanceSet)
                {
                    enemy.SetAnimSpeed(0.25f);
                    enemy.SetSpeed(enemy.idleSpeed);
                    distanceSet = true;
                }
            }
            else if((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude > enemy.rangeBeforeAttack + 5)
            {
                if (distanceSet)
                {
                    enemy.SetAnimSpeed(0.75f);
                    enemy.SetSpeed(enemy.runSpeed);
                    randomDistance = Random.Range(enemy.rangeBeforeAttack + 1, enemy.rangeBeforeAttack + 5);
                    distanceSet = false;
                }
            }

            if (Mathf.Abs(enemy.movementDircetion.magnitude) > 0.01f)
            {
                enemy.RotateToPosition(enemy.transform.position + enemy.movementDircetion);
            }

            if (!scanning)
            {
                ScanArea();
                enemy.DelayedCallback(enemy.runState, "ScanningDone", enemy.timeBetweenScan);
            }
            
        }

        if (!enemy.inView)
        {
            if(timer < enemy.timeBeforeLost)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = enemy.timeBeforeLost;
                enemy.SetPointOfInterest(GameManager.instance.cam.transform.position);
                enemy.voiceLines.RunSearching();
                enemy.SwitchState(enemy.searchingState);
            }
            
        }
        else
        {

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
            }

        }

        enemy.LookingForPlayer(enemy.runSightRange);





        //Switch to other states
        if (enemy.stunned)
        {
            enemy.SwitchState(enemy.stunnedState);
        }
        if (enemy.hiding)
        {
            enemy.SwitchState(enemy.coverState);
        }
    }

    private void ScanArea()
    {
        scanning = true;
        if (!runningEnemy.justKicked)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i] = null;
            }

            int hits = Physics.OverlapSphereNonAlloc(agent.transform.position, runningEnemy.scanRadius, colliders, runningEnemy.scanableLayer);


            int hitReduction = 0;
            for (int i = 0; i < hits; i++)
            {
                if (colliders[i].GetComponent<Kickable>() == null)
                {
                    colliders[i] = null;
                    hitReduction++;
                }
            }
            hits -= hitReduction;

            System.Array.Sort(colliders, ColliderArraySortComparer);

            for (int i = 0; i < hits; i++)
            {
                if (!colliders[i].GetComponent<Kickable>().isBeeingKicked)
                {
                    if ((GameManager.instance.XROrigin.transform.position - agent.transform.position).magnitude > (colliders[i].transform.position - agent.transform.position).magnitude)
                    {
                        if (Vector3.Angle(colliders[i].transform.position - agent.transform.position, runningEnemy.transform.forward) <= runningEnemy.FOV * 0.5f)
                        {
                            Vector3 pointOnNavMesh;
                            NavMeshHit myNavHit;
                            if (NavMesh.SamplePosition(colliders[i].transform.position, out myNavHit, 10, NavMesh.AllAreas))
                            {
                                pointOnNavMesh = myNavHit.position;
                                NavMeshHit hit;
                                if (NavMesh.FindClosestEdge(pointOnNavMesh, out hit, NavMesh.AllAreas))
                                {
                                    if ((hit.position - colliders[i].transform.position).magnitude >= runningEnemy.barrelDistance)
                                    {
                                        currentKickable = colliders[i].GetComponent<Kickable>();
                                        currentKickable.IsBeeingKicked(true);
                                        runningEnemy.SetKickable(currentKickable);
                                        runningEnemy.SwitchState(runningEnemy.kickState);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
    }
    public void ScanningDone()
    {
        scanning = false;
    }
    private int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(agent.transform.position, A.transform.position).CompareTo(Vector3.Distance(agent.transform.position, B.transform.position));
        }
    }
    private bool CheckForEnemies()
    {
        RaycastHit hit;
        if (Physics.Raycast(runningEnemy.headTrans.position + new Vector3(0, 0.5f, 0f), GameManager.instance.XROrigin.transform.position - runningEnemy.headTrans.position, out hit, 2, runningEnemy.enemyLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.GetComponentInParent<RunningEnemy>() != runningEnemy)
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
}
