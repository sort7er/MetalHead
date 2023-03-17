using UnityEngine;

public class EnemyKickState : EnemyBaseState
{
    private Vector3 kickPosition, directionFromPlayer, kickDirection;
    private Kickable kickableToKick;
    private Rigidbody rb;
    private float kickForce;
    private bool arrivedToKickable, kickDone;
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state kick");
        enemy.agent.ResetPath();
        kickForce = enemy.kickForce;
        kickableToKick = enemy.currentKickable;
        rb = kickableToKick.GetComponent<Rigidbody>();
        arrivedToKickable = false;
        kickDone = false;
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        directionFromPlayer = GameManager.instance.XROrigin.transform.position - kickableToKick.transform.position;
        kickPosition = kickableToKick.transform.position - new Vector3(directionFromPlayer.normalized.x, kickableToKick.transform.position.y, directionFromPlayer.normalized.z);
        kickDirection = new Vector3(directionFromPlayer.normalized.x, directionFromPlayer.normalized.y + 0.7f, directionFromPlayer.normalized.z);
        if (!arrivedToKickable)
        {
            enemy.agent.SetDestination(kickPosition);
            enemy.RotateToPosition(kickPosition);
            if((kickPosition - enemy.transform.position).magnitude < 0.2f)
            {
                arrivedToKickable = true;
                enemy.JustKicked();
                enemy.DelayedCallback(enemy.kickState, "Kick", 0.1f);
                enemy.DelayedCallback(enemy.kickState, "KickDone", 0.3f);
            }
        }
        else
        {
            enemy.RotateToPosition(kickableToKick.transform.position);
        }
        if (kickDone)
        {
            kickDone = false;
            enemy.SwitchState(enemy.runState);
        }
    }
    public void Kick()
    {
        rb.AddForce(kickDirection * kickForce, ForceMode.Impulse);
    }
    public void KickDone()
    {
        kickDone = true;
    }
}
