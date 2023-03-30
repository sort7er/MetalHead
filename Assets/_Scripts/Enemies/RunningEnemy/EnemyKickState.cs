using UnityEngine;
using UnityEngine.AI;

public class EnemyKickState : EnemyBaseState
{
    private RunningEnemy runningEnemy;
    private Vector3 kickPosition, directionFromPlayer, kickDirection;
    private Kickable kickableToKick;
    private Rigidbody rb;
    private float kickForce;
    private bool arrivedToKickable, abort;
    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state kick");
        enemy.rig.SetRig(true);
        runningEnemy = enemy;
        enemy.agent.ResetPath();
        kickForce = enemy.kickForce;
        kickableToKick = enemy.currentKickable;
        rb = kickableToKick.GetComponent<Rigidbody>();
        arrivedToKickable = false;
        abort = false;
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        directionFromPlayer = GameManager.instance.XROrigin.transform.position - kickableToKick.transform.position;
        kickPosition = kickableToKick.transform.position - new Vector3(directionFromPlayer.normalized.x, 0, directionFromPlayer.normalized.z);
        kickDirection = new Vector3(directionFromPlayer.normalized.x, directionFromPlayer.normalized.y + 0.7f, directionFromPlayer.normalized.z);
        enemy.rig.SetTarget(GameManager.instance.cam.transform.position);
        if (!arrivedToKickable)
        {
            enemy.SetNavMeshDestination(kickPosition);
            
            enemy.RotateToPosition(kickPosition);
            if((kickPosition - enemy.transform.position).magnitude < 0.5f)
            {
                arrivedToKickable = true;
                enemy.SetTurnSpeed(10);
                enemy.DelayedCallback(enemy.kickState, "KickStartUp", 0.1f);
                enemy.DelayedCallback(enemy.kickState, "Kick", 0.2f);
                enemy.DelayedCallback(enemy.kickState, "KickCanDamage", 0.3f);
                enemy.DelayedCallback(enemy.kickState, "KickDone", 0.4f);
            }
            if ((kickPosition - enemy.transform.position).magnitude > 3f || (GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude > 10f || CheckIfTooCloseTooWall() && !abort)
            {
                abort = true;
                kickableToKick.IsBeeingKicked(false);
                KickDone();
                runningEnemy.JustKicked();
            }
        }
        else
        {
            enemy.RotateToPosition(kickableToKick.transform.position);
        }
        
        //Switch to other states
        if (enemy.stunned)
        {
            kickableToKick.IsBeeingKicked(false);
            enemy.SwitchState(enemy.stunnedState);
        }
    }
    public void KickStartUp()
    {
        runningEnemy.enemyAnim.SetTrigger("Kick");
        runningEnemy.JustKicked();
    }
    public void Kick()
    {
        rb.AddForce(kickDirection * kickForce, ForceMode.Impulse);

    }
    public void KickCanDamage()
    {
        kickableToKick.CanDamage();
    }
    public void KickDone()
    {
        kickableToKick.IsBeeingKicked(false);
        runningEnemy.SwitchState(runningEnemy.runState);
    }
    private bool CheckIfTooCloseTooWall()
    {
        Vector3 pointOnNavMesh;
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(kickableToKick.transform.position, out myNavHit, 10, NavMesh.AllAreas))
        {
            pointOnNavMesh = myNavHit.position;
            NavMeshHit hit;
            if (NavMesh.FindClosestEdge(pointOnNavMesh, out hit, NavMesh.AllAreas))
            {
                if ((hit.position - kickableToKick.transform.position).magnitude >= runningEnemy.barrelDistance)
                {
                    return false;
                }
                else
                {
                    Debug.Log("Too close1");
                    return true;
                }
            }
            else
            {
                Debug.Log("Too close2");
                return true;
            }
        }
        else
        {
            Debug.Log("Too close3");
            return true;
        }
    }
}
