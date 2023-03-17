using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyRunState : EnemyBaseState
{
    private float timer, scanRadius;
    private bool scanning, startKick, justKicked;
    private NavMeshAgent agent;
    private LayerMask scanableLayer;
    private Kickable currentKickable;
    private Collider[] colliders = new Collider[3];

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state run");
        agent = enemy.agent;
        agent.ResetPath();
        scanRadius = enemy.scanRadius;
        scanableLayer = enemy.scanableLayer;
        enemy.SetGlowColor(enemy.detectedColor);
        enemy.SetSpeed(enemy.runSpeed);
        enemy.PlayerInSight(true);
        enemy.PlayerDetected();
        timer = 0;
        scanning = false;
        currentKickable = null;

    }

    public override void UpdateState(RunningEnemy enemy)
    {
        justKicked = enemy.justKicked;

        if((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= enemy.rangeBeforeAttack && enemy.CheckLineOfSight(true, enemy.transform.forward, enemy.transform.position + new Vector3(0, 0.5f, 0)))
        {
            enemy.SwitchState(enemy.attackState);
            enemy.agent.ResetPath();
        }
        else
        {
            enemy.agent.SetDestination(GameManager.instance.XROrigin.transform.position);
            if (enemy.CheckLineOfSight(false, enemy.directionToPlayer, enemy.headTrans.position))
            {
                enemy.RotateToPosition(GameManager.instance.XROrigin.transform.position);
            }
            else if(Mathf.Abs(enemy.movementDircetion.magnitude) > 0.01f)
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
                enemy.SetPointOfInterest(GameManager.instance.XROrigin.transform.position);
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

        if (startKick)
        {
            enemy.SetKickable(currentKickable);
            startKick = false;
            enemy.SwitchState(enemy.kickState);
        }
    }

    private void ScanArea()
    {
        scanning = true;
        if (!justKicked)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i] = null;
            }

            int hits = Physics.OverlapSphereNonAlloc(agent.transform.position, scanRadius, colliders, scanableLayer);


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
                if ((GameManager.instance.XROrigin.transform.position - agent.transform.position).magnitude > (colliders[i].transform.position - agent.transform.position).magnitude)
                {
                    if ((GameManager.instance.XROrigin.transform.position - colliders[i].transform.position).magnitude < (agent.transform.position - colliders[i].transform.position).magnitude * 1.5f)
                    {
                        currentKickable = colliders[i].GetComponent<Kickable>();
                        startKick = true;
                        break;
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
}
