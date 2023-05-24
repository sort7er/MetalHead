using System.IO;
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
    private NavMeshPath path;
    private float checkCoverRadius, minPlayerDistance;
    private bool hideLocked, inCover;
    private int hitsMissed, healthWhenInCover;

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
        colliderChosen = null;
        inCover = false;
        hideLocked = false;
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(runningEnemy.transform.position, out myNavHit, 1, NavMesh.AllAreas))
        {
            runningEnemy.transform.position = myNavHit.position;
        }

        //Hide(GameManager.instance.XROrigin.transform);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if (!inCover)
        {
            if ((destination - enemy.transform.position).magnitude < 0.2f && colliderChosen != null)
            {
                InCover();
                enemy.DelayedCallback(enemy.coverState, "OutOfCover", Random.Range(enemy.minCoverDuration, enemy.maxCoverDuration));
            }
            else
            {
                Hide(GameManager.instance.XROrigin.transform);
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
        else
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
                //else
                //{
                //    for (int j = 0; j < AIManager.instance.runningEnemiesInScene.Length; j++)
                //    {
                //        if (AIManager.instance.runningEnemiesInScene[j] != null && (AIManager.instance.runningEnemiesInScene[j].transform.position - colliders[i].transform.position).magnitude < 1.5f)
                //        {
                //            colliders[i] = null;
                //            hitReduction++;
                //            break;
                //        }
                //    }
                //}
            }
            hits -= hitReduction;
            Debug.Log(hits);



            if(hits == 0)
            {
                runningEnemy.SwitchState(runningEnemy.dodgeState);
            }
            else
            {
                System.Array.Sort(colliders, ColliderArraySortComparer);

                hitsMissed = 0;

                for (int i = 0; i < hits; i++)
                {
                    Debug.Log("lol");
                    if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, 2f, agent.areaMask))
                    {
                        if (Vector3.Dot(hit.normal, (target.position - hit.position).normalized) < runningEnemy.hideSensitivity)
                        {
                            destination = hit.position;
                            colliderChosen = colliders[i];
                            runningEnemy.SetNavMeshDestination(destination);
                            //runningEnemy.voiceLines.Hiding();
                            break;
                        }
                        else
                        {
                            if (NavMesh.SamplePosition(colliders[i].transform.position - (target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, agent.areaMask))
                            {
                                if (Vector3.Dot(hit2.normal, (target.position - hit2.position).normalized) < runningEnemy.hideSensitivity)
                                {

                                    destination = hit2.position;
                                    colliderChosen = colliders[i];
                                    runningEnemy.SetNavMeshDestination(destination);
                                    //runningEnemy.voiceLines.Hiding();
                                    break;
                                }
                                else
                                {
                                    //hitsMissed++;
                                    //if(hitsMissed >= hits)
                                    //{
                                    //    Dodge();
                                    //    break;
                                    //}
                                }
                            }
                            //hitsMissed++;
                            //if (hitsMissed >= hits)
                            //{
                            //    Dodge();
                            //    break;
                        }
                    }
                    else
                    {
                        //hitsMissed++;
                        //if (hitsMissed >= hits)
                        //{
                        //    Dodge();
                        //    break;
                        //}
                        //Debug.Log("Unable to find NavMesh near object " + colliders[i].name + " at " + colliders[i].transform.position);
                    }
                }
            }
            hideLocked= true;
            runningEnemy.DelayedCallback(runningEnemy.coverState, "HideUnlocked", runningEnemy.defaultTimeCoverCheck);
        } 
    }
    public bool CalculateNewPath(Vector3 targetPos)
    {
        path = new NavMeshPath();
        agent.CalculatePath(targetPos, path);
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else
        {
            return true;
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
