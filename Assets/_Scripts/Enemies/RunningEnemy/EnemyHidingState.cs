using UnityEngine;
using UnityEngine.AI;

public class EnemyHidingState : EnemyBaseState
{
    private RunningEnemy runningEnemy;
    private NavMeshAgent agent;
    private Collider[] colliders = new Collider[100];
    private Animator enemyAnim;
    private Vector3 destination;
    private float checkCoverRadius, minPlayerDistance;
    private bool inCover, noDodge, knockbackDone, invokeStarted;
    private int healthWhenInCover;

    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state cover");
        runningEnemy = enemy;
        agent = enemy.agent;
        agent.avoidancePriority = 49;
        enemyAnim = enemy.enemyAnim;
        enemyAnim.SetBool("IsMoving", true);
        enemyAnim.SetBool("LookingAround", false);
        enemy.SetAnimSpeed(1f);
        agent.speed = enemy.coverSpeed;
        checkCoverRadius = enemy.checkCoverRadius;
        minPlayerDistance = enemy.minPlayerDistance;
        agent.ResetPath();
        inCover = false;
        invokeStarted = false;
        noDodge = false;
        knockbackDone= false;
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(runningEnemy.transform.position, out myNavHit, 0.5f, agent.areaMask))
        {
            agent.Warp(myNavHit.position);
        }
        
        
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if (!runningEnemy.knockBack && !knockbackDone)
        {
            Hide(GameManager.instance.XROrigin.transform);
            knockbackDone = true;
        }

        if (!inCover)
        {
            NavMeshHit myNavHit;
            if ((destination - enemy.transform.position).magnitude < 0.2f)
            {

                if(runningEnemy.hiding)
                {
                    InCover();
                    enemy.DelayedCallback(enemy.coverState, "OutOfCover", Random.Range(enemy.minCoverDuration, enemy.maxCoverDuration));
                }
                else
                {
                    runningEnemy.SwitchState(runningEnemy.runState);
                }
                
            }
            else if((destination - enemy.transform.position).magnitude < 1.2f && !invokeStarted && runningEnemy.hiding)
            {
                enemy.DelayedCallback(enemy.coverState, "InCover", enemy.minCoverDuration * 0.1f);
                invokeStarted = true;
            }
            else
            {
                if (Mathf.Abs(enemy.movementDircetion.magnitude) > 0.1f)
                {
                    enemy.RotateToPosition(enemy.transform.position + enemy.movementDircetion);
                }
            }
        }
        else
        {
            enemy.LookingForPlayer(enemy.sightRangeForCover);
            if (enemy.inView || healthWhenInCover != runningEnemy.health.GetCurrentHealth())
            {
                OutOfCover();
            }
        }


        
    }

    public void Hide(Transform target)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i] = null;
        }

        int hits = Physics.OverlapSphereNonAlloc(agent.transform.position, checkCoverRadius, colliders, runningEnemy.hidebleLayer);

        int hitReduction = 0;
        for (int i = 0; i < hits; i++)
        {
            if ((target.position - colliders[i].transform.position).magnitude < minPlayerDistance)
            {
                colliders[i] = null;
                hitReduction++;
            }
        }
        hits -= hitReduction;

        System.Array.Sort(colliders, ColliderArraySortComparer);

        for (int i = 0; i < hits; i++)
        {
            if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, 2f, agent.areaMask))
            {
                if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                {
                    Debug.Log("No nav mesh at " + hit.position);
                }
                if (Vector3.Dot(hit.normal, (target.position - hit.position).normalized) < runningEnemy.hideSensitivity)
                {
                    if (runningEnemy.hiding)
                    {
                        runningEnemy.voiceLines.Hiding();
                    }
                    noDodge = true;
                    destination = hit.position;
                    agent.SetDestination(destination);
                    break;
                }
                else
                {
                    if (NavMesh.SamplePosition(colliders[i].transform.position - (target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, agent.areaMask))
                    {
                        if (!NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask))
                        {
                            Debug.Log("No nav mesh at " + hit2.position + "(second attept)");
                        }
                        if (Vector3.Dot(hit2.normal, (target.position - hit2.position).normalized) < runningEnemy.hideSensitivity)
                        {
                            if (runningEnemy.hiding)
                            {
                                runningEnemy.voiceLines.Hiding();
                            }
                            destination = hit2.position;
                            agent.SetDestination(destination);
                            noDodge = true;
                            break;
                        }
                    }
                }
            }
        }
        Debug.Log(noDodge);
        if(!noDodge)
        {
            runningEnemy.SwitchState(runningEnemy.dodgeState);
        }
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
    public void InCover()
    {
        inCover = true;
        healthWhenInCover = runningEnemy.health.GetCurrentHealth();
        enemyAnim.SetBool("InCover", true);
        agent.avoidancePriority = 48;
    }
    public void OutOfCover()
    {
        enemyAnim.SetBool("InCover", false);
        runningEnemy.DelayedCallback(runningEnemy.coverState, "HidingDone", 0.3f);
    }

    public void HidingDone()
    {
        runningEnemy.IsHiding(false);
        if (runningEnemy.playerInSight)
        {
            runningEnemy.SwitchState(runningEnemy.runState);

        }
        else
        {
            runningEnemy.SwitchState(runningEnemy.searchingState);
        }
    }
}
