using UnityEngine;

public class EnemyKickState : EnemyBaseState
{
    private RunningEnemy runningEnemy;
    private Vector3 kickPosition, directionFromPlayer, kickDirection;
    private Kickable kickableToKick;
    private Rigidbody rb;
    private float kickForce;
    private bool arrivedToKickable;
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state kick");
        enemy.rig.SetRig(true);
        runningEnemy = enemy;
        enemy.agent.ResetPath();
        kickForce = enemy.kickForce;
        kickableToKick = enemy.currentKickable;
        rb = kickableToKick.GetComponent<Rigidbody>();
        arrivedToKickable = false;
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        directionFromPlayer = GameManager.instance.XROrigin.transform.position - kickableToKick.transform.position;
        kickPosition = kickableToKick.transform.position - new Vector3(directionFromPlayer.normalized.x, kickableToKick.transform.position.y, directionFromPlayer.normalized.z);
        kickDirection = new Vector3(directionFromPlayer.normalized.x, directionFromPlayer.normalized.y + 0.7f, directionFromPlayer.normalized.z);
        enemy.rig.SetTarget(GameManager.instance.cam.transform.position);
        if (!arrivedToKickable)
        {
            enemy.SetNavMeshDestination(kickPosition);
            
            enemy.RotateToPosition(kickPosition);
            if((kickPosition - enemy.transform.position).magnitude < 0.3f)
            {
                arrivedToKickable = true;
                enemy.SetTurnSpeed(10);
                enemy.DelayedCallback(enemy.kickState, "KickStartUp", 0.1f);
                enemy.DelayedCallback(enemy.kickState, "Kick", 0.2f);
                enemy.DelayedCallback(enemy.kickState, "KickDone", 0.4f);
            }
            if ((kickPosition - enemy.transform.position).magnitude > 5f)
            {
                kickableToKick.IsBeeingKicked(false);
                KickDone();
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
        kickableToKick.IsBeeingKicked(false);
    }
    public void KickDone()
    {
        runningEnemy.SwitchState(runningEnemy.runState);
    }
}
