using UnityEngine;
using UnityEngine.AI;

public class EnemyCoverState : EnemyBaseState
{
    private NavMeshAgent agent;
    private LayerMask hidebleLayer;
    private Collider[] colliders = new Collider[10];
    private Collider colliderChosen;
    private Vector3 destination;
    private float hideSensitivity, checkCoverRadius, minPlayerDistance;

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
        colliderChosen = null;
        Hide(GameManager.instance.XROrigin.transform);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        Hide(GameManager.instance.XROrigin.transform);
        hideSensitivity = enemy.hideSensitivity;
        if ((destination - enemy.transform.position).magnitude < 1.1f)
        {
            if(colliderChosen.transform.localScale.y < 1.5f)
            {
                Debug.Log("Gotta duck");
            }
            else
            {
                Debug.Log("Can stand");
            }
        }
        else
        {
            //Rotate to point of interest
            if (enemy.CheckLineOfSight(true, enemy.pointOfInterest))
            {
                enemy.RotateToPosition(enemy.pointOfInterest);
            }
            else
            {
                enemy.RotateToPosition(enemy.transform.position + enemy.movementDircetion);
            }
        }
    }

    public void Hide(Transform target)
    {
        
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i] = null;
        }


        int hits = Physics.OverlapSphereNonAlloc(agent.transform.position, checkCoverRadius, colliders, hidebleLayer);

        int hitReduction = 0;
        for(int i = 0; i < hits; i++)
        {
            if ((target.position - colliders[i].transform.position).magnitude < minPlayerDistance)
            {
                colliders[i] = null;
                hitReduction++;
            }
        }
        hits -= hitReduction;
        


        System.Array.Sort(colliders, ColliderArraySortComparer);

        for(int i = 0; i < hits; i++)
        {
            if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, 2f, agent.areaMask))
            {
                if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                {
                    Debug.Log("Unable to find closest edge close to " + hit.position);
                }
                if(Vector3.Dot(hit.normal, (target.position - hit.position).normalized) < hideSensitivity)
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
}
