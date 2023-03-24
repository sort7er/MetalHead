using UnityEngine;
using UnityEngine.AI;

public class EnemyCoverState : EnemyBaseState
{
    private RunningEnemy runningEnemy;
    private NavMeshAgent agent;
    private Collider[] colliders = new Collider[10];
    private Collider colliderChosen;
    private Animator enemyAnim;
    private Vector3 destination;
    private float checkCoverRadius, minPlayerDistance, dodgeSpeed;
    private bool hideLocked, inCover, dodge;
    private int hitsMissed;

    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state cover");
        runningEnemy = enemy;
        agent = enemy.agent;
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
        hideLocked= false;
        Hide(GameManager.instance.XROrigin.transform);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        
        if (dodge && (destination - enemy.transform.position).magnitude < 0.2f)
        {
            agent.ResetPath();
            HidingDone();
            
        }

        if (!inCover && !dodge)
        {
            if ((destination - enemy.transform.position).magnitude < 0.2f && colliderChosen != null)
            {
                InCover();
                enemy.DelayedCallback(enemy.coverState, "OutOfCover", Random.Range(enemy.minCoverDuration, enemy.maxCoverDuration));
            }
            else
            {
                Hide(GameManager.instance.XROrigin.transform);
                enemy.RotateToPosition(destination);

            }
        }
        else if (inCover && !dodge)
        {
            enemy.LookingForPlayer(enemy.sightRangeForCover);
            if (enemy.inView)
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
        if (!hideLocked)
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
                        }
                        if (Vector3.Dot(hit.normal, (target.position - hit.position).normalized) < runningEnemy.hideSensitivity)
                        {
                            destination = hit.position;
                            colliderChosen = colliders[i];
                            runningEnemy.SetNavMeshDestination(destination);
                            break;
                        }
                        else
                        {
                            if (NavMesh.SamplePosition(colliders[i].transform.position - (target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, agent.areaMask))
                            {
                                if (!NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask))
                                {
                                    Debug.Log("Unable to find closest edge close to " + hit2.position);
                                }
                                if (Vector3.Dot(hit2.normal, (target.position - hit2.position).normalized) < runningEnemy.hideSensitivity)
                                {
                                    destination = hit2.position;
                                    colliderChosen = colliders[i];
                                    runningEnemy.SetNavMeshDestination(destination);
                                    break;
                                }
                                else
                                {
                                    hitsMissed++;
                                    if(hitsMissed >= hits)
                                    {
                                        Dodge();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Unable to find NavMesh near object " + colliders[i].name + " at " + colliders[i].transform.position);
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
        enemyAnim.SetBool("InCover", true);
    }
    public void OutOfCover()
    {
        enemyAnim.SetBool("InCover", false);
        runningEnemy.DelayedCallback(runningEnemy.coverState, "HidingDone", 0.3f);
    }
    private void Dodge()
    {
        dodge = true;
        agent.speed = dodgeSpeed;
        int direction = Random.Range(0, 2);
        if(direction == 0)
        {
            enemyAnim.SetTrigger("DodgeRight");
            destination = runningEnemy.transform.position + runningEnemy.transform.right;
            
        }
        else if (direction == 1)
        {
            enemyAnim.SetTrigger("DodgeLeft");
            destination = runningEnemy.transform.position - runningEnemy.transform.right;
        }
        runningEnemy.DelayedCallback(runningEnemy.coverState, "StartDodge", 0.15f);
    }
    public void StartDodge()
    {
        runningEnemy.SetNavMeshDestination(destination);
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
