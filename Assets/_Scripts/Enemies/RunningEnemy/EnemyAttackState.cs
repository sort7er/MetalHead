using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state attacking");
        enemy.agent.ResetPath();
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if ((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude > enemy.rangeBeforeAttack || !enemy.CheckLineOfSight(false, enemy.directionToPlayer))
        {
            enemy.SwitchState(enemy.runState);
        }
        else
        {
            Attack();
        }
        enemy.RotateToPosition(GameManager.instance.XROrigin.transform.position);
    }

    private void Attack()
    {
        Debug.Log("Attacking player");
    }
}
