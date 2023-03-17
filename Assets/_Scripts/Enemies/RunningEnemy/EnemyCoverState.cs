using UnityEngine;
using UnityEngine.AI;

public class EnemyCoverState : EnemyBaseState
{
    private NavMeshAgent agent;
    private LayerMask hidebleLayer;
    private Collider[] colliders = new Collider[10];
    private Collider colliderChosen;
    private Vector3 destination;
    private Transform enemyTrans;
    private float hideSensitivity, checkCoverRadius, minPlayerDistance, dodgeSpeed;
    private bool hideLocked, relay, inCover, outOfCover, dodge;

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state cover");
        agent = enemy.agent;
        agent.speed = enemy.coverSpeed;
        hidebleLayer = enemy.hidebleLayer;
        hideSensitivity= enemy.hideSensitivity;
        checkCoverRadius= enemy.checkCoverRadius;
        minPlayerDistance = enemy.minPlayerDistance;
        agent.ResetPath();
        dodgeSpeed = enemy.dodgeSpeed;
        colliderChosen = null;
        inCover = false;
        outOfCover= false;
        dodge= false;
        hideLocked= false;
        relay = false;
        Hide(GameManager.instance.XROrigin.transform);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        enemyTrans = enemy.transform;
        if (outOfCover)
        {
            outOfCover = false;
            enemy.SwitchState(enemy.runState);
        }
        
        if (dodge && (destination - enemyTrans.position).magnitude < 0.2f)
        {
            agent.ResetPath();
            dodge = false;
            OutOfCover();
            
        }

        if (!inCover && !dodge)
        {
            hideSensitivity = enemy.hideSensitivity;
            if ((destination - enemyTrans.position).magnitude < 0.2f && colliderChosen != null)
            {
                InCover();
                enemy.DelayedCallback(enemy.coverState, "OutOfCover", Random.Range(enemy.minCoverDuration, enemy.maxCoverDuration));
            }
            else
            {
                Hide(GameManager.instance.XROrigin.transform);
                enemy.RotateToPosition(destination);

            }
            if (relay)
            {
                enemy.DelayedCallback(enemy.coverState, "HideUnlocked", enemy.defaultTimeCoverCheck);
                relay = false;
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
    }

    public void Hide(Transform target)
    {
        if (!hideLocked)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i] = null;
            }

            

            int hits = Physics.OverlapSphereNonAlloc(agent.transform.position, checkCoverRadius, colliders, hidebleLayer);

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

                for (int i = 0; i < hits; i++)
                {
                    if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, 2f, agent.areaMask))
                    {
                        if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                        {
                            Debug.Log("Unable to find closest edge close to " + hit.position);
                        }
                        if (Vector3.Dot(hit.normal, (target.position - hit.position).normalized) < hideSensitivity)
                        {
                            destination = hit.position;
                            colliderChosen = colliders[i];
                            agent.SetDestination(destination);
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
                                if (Vector3.Dot(hit2.normal, (target.position - hit2.position).normalized) < hideSensitivity)
                                {
                                    destination = hit2.position;
                                    colliderChosen = colliders[i];
                                    agent.SetDestination(destination);
                                    break;
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
            relay = true;
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
        inCover= true;
        if (colliderChosen.transform.localScale.y < 1.8f)
        {
            Debug.Log("Gotta duck");
        }
        else
        {
            Debug.Log("Can stand");
        }
    }
    public void OutOfCover()
    {
        inCover= false;
        outOfCover= true;
    }
    private void Dodge()
    {
        dodge = true;
        agent.speed = dodgeSpeed;
        int direction = Random.Range(0, 2);
        if(direction == 0)
        {
            destination = enemyTrans.position + enemyTrans.right;
            
        }
        else if (direction == 1)
        {
            destination = enemyTrans.position - enemyTrans.right;
        }
        agent.SetDestination(destination);
    }
}
