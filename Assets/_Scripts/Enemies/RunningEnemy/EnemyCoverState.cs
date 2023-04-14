using UnityEngine;
using UnityEngine.AI;

public class EnemyCoverState : EnemyBaseState
{
    private RunningEnemy runningEnemy;
    private NavMeshAgent agent;
    private Collider[] colliders = new Collider[10];
    private Collider[] enemies = new Collider[5];
    private Collider colliderChosen;
    private Animator enemyAnim;
    private Vector3 destination, dodgeDestination;
    private float checkCoverRadius, minPlayerDistance, dodgeSpeed;
    private bool hideLocked, inCover, dodge, canLeft, canRight;
    private int hitsMissed, direction, healthWhenInCover;

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
        checkCoverRadius= enemy.checkCoverRadius;
        minPlayerDistance = enemy.minPlayerDistance;
        agent.ResetPath();
        dodgeSpeed = enemy.dodgeSpeed;
        colliderChosen = null;
        inCover = false;
        dodge= false;
        hideLocked = false;
        canLeft = false;
        canRight = false;
        direction = 0;
        Hide(GameManager.instance.XROrigin.transform);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if (!inCover && !dodge)
        {
            if ((destination - enemy.transform.position).magnitude < 0.2f && colliderChosen != null)
            {
                InCover();
                enemy.DelayedCallback(enemy.coverState, "OutOfCover", Random.Range(enemy.minCoverDuration, enemy.maxCoverDuration));
            }
            else
            {
                //Hide(GameManager.instance.XROrigin.transform);
                if (enemy.CheckLineOfSight(true, destination - enemy.transform.position, enemy.transform.position + new Vector3(0, 0.5f, 0)))
                {
                    enemy.RotateToPosition(destination);
                }
                else if (Mathf.Abs(enemy.movementDircetion.magnitude) > 0.01f)
                {
                    enemy.RotateToPosition(enemy.transform.position + enemy.movementDircetion);
                }
                

            }
        }
        else if (inCover && !dodge)
        {
            enemy.LookingForPlayer(enemy.sightRangeForCover);
            if (enemy.inView || healthWhenInCover != runningEnemy.health.GetCurrentHealth())
            {
                OutOfCover();
            }
        }

        //Switch to other states
        if (enemy.stunned)
        {
            runningEnemy.IsHiding(false);
            enemy.SwitchState(enemy.stunnedState);
        }
    }

    public void Hide(Transform target)
    {
        if (!hideLocked && !dodge)
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
                else
                {
                    for (int j = 0; j < AIManager.instance.runningEnemiesInScene.Length; j++)
                    {
                        if (AIManager.instance.runningEnemiesInScene[j] != null && (AIManager.instance.runningEnemiesInScene[j].transform.position - colliders[i].transform.position).magnitude < 1.5f)
                        {
                            colliders[i] = null;
                            hitReduction++;
                            break;
                        }
                    }
                }
            }
            hits -= hitReduction;

            if(hits == 0)
            {
                Dodge();
            }
            else
            {
                System.Array.Sort(colliders, ColliderArraySortComparer);

                hitsMissed = 0;

                for (int i = 0; i < hits; i++)
                {
                    if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, 2f, agent.areaMask))
                    {
                        if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                        {
                            Debug.Log("Unable to find closest edge close to " + hit.position);
                            Dodge();
                            break;
                        }
                        if (Vector3.Dot(hit.normal, (target.position - hit.position).normalized) < runningEnemy.hideSensitivity)
                        {
                            destination = hit.position;
                            colliderChosen = colliders[i];
                            runningEnemy.SetNavMeshDestination(destination);
                            runningEnemy.voiceLines.Hiding();
                            break;
                        }
                        else
                        {
                            if (NavMesh.SamplePosition(colliders[i].transform.position - (target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, agent.areaMask))
                            {
                                if (!NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask))
                                {
                                    Debug.Log("Unable to find closest edge close to " + hit2.position);
                                    Dodge();
                                    break;
                                }
                                if (Vector3.Dot(hit2.normal, (target.position - hit2.position).normalized) < runningEnemy.hideSensitivity)
                                {
                                    destination = hit2.position;
                                    colliderChosen = colliders[i];
                                    runningEnemy.SetNavMeshDestination(destination);
                                    runningEnemy.voiceLines.Hiding();
                                    break;
                                }
                                else
                                {
                                    hitsMissed++;
                                    if(hitsMissed >= hits)
                                    {
                                        Dodge();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Dodge();
                                break;
                            }
                        }
                    }
                    else
                    {
                        Dodge();
                        Debug.Log("Unable to find NavMesh near object " + colliders[i].name + " at " + colliders[i].transform.position);
                        break;
                    }
                }
            }
            hideLocked= true;
            runningEnemy.DelayedCallback(runningEnemy.coverState, "HideUnlocked", runningEnemy.defaultTimeCoverCheck);
        } 
    }
    private int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if(A != null && B == null)
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

    public void HideUnlocked()
    {
        hideLocked = false;
    }
    private void InCover()
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
    private void Dodge()
    {
        if (!dodge)
        {
            NavMeshHit navHitRight;
            if (NavMesh.SamplePosition(runningEnemy.transform.position + runningEnemy.transform.right, out navHitRight, 0.5f, NavMesh.AllAreas) && !Physics.Raycast(runningEnemy.transform.position + new Vector3(0, 0.5f, 0), runningEnemy.transform.right, 1, runningEnemy.hidebleLayer))
            {
                canRight = true;
            }
            NavMeshHit navHitLeft;
            if (NavMesh.SamplePosition(runningEnemy.transform.position - runningEnemy.transform.right, out navHitLeft, 0.5f, NavMesh.AllAreas) && !Physics.Raycast(runningEnemy.transform.position + new Vector3(0, 0.5f, 0), -runningEnemy.transform.right, 1, runningEnemy.hidebleLayer))
            {
                canLeft = true;
            }
            if (!canLeft && !canRight)
            {
                HidingDone();
            }
            else
            {
                if (canLeft && canRight)
                {
                    direction = Random.Range(0, 2);
                }
                else if (!canLeft && canRight)
                {
                    direction = 0;
                }
                else if (canLeft && !canRight)
                {
                    direction = 1;
                }

                if (direction == 0)
                {
                    runningEnemy.ChangeAnimationState("DodgeRight");
                    dodgeDestination = runningEnemy.transform.position + runningEnemy.transform.right;

                }
                else if (direction == 1)
                {
                    runningEnemy.ChangeAnimationState("DodgeLeft");
                    dodgeDestination = runningEnemy.transform.position - runningEnemy.transform.right;
                }
                agent.speed = dodgeSpeed;
                runningEnemy.agent.SetDestination(dodgeDestination);
                dodge = true;
                runningEnemy.DelayedCallback(runningEnemy.coverState, "StartDodge", 0.5f);
            }
        }
    }
    public void StartDodge()
    {
        agent.ResetPath();
        runningEnemy.DelayedCallback(runningEnemy.coverState, "HidingDone", 0.5f);
        enemyAnim.SetBool("IsMoving", false);
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
