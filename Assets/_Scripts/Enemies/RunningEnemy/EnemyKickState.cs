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
        if (!arrivedToKickable)
        {
            enemy.SetNavMeshDestination(kickPosition);
            enemy.rig.SetTarget(GameManager.instance.cam.transform.position);
            enemy.RotateToPosition(kickPosition);
            if((kickPosition - enemy.transform.position).magnitude < 0.2f)
            {
                arrivedToKickable = true;
                enemy.enemyAnim.SetBool("IsMoving", false);
                enemy.JustKicked();
                enemy.DelayedCallback(enemy.kickState, "Kick", 0.1f);
                enemy.DelayedCallback(enemy.kickState, "KickDone", 0.3f);
            }
        }
        else
        {
            enemy.rig.SetTarget(kickPosition);
            enemy.RotateToPosition(kickableToKick.transform.position);
        }
    }
    public void Kick()
    {
        rb.AddForce(kickDirection * kickForce, ForceMode.Impulse);
    }
    public void KickDone()
    {
        runningEnemy.SwitchState(runningEnemy.runState);
    }
}
