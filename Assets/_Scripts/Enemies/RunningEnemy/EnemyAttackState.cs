using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private Animator enemyAnim;
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state attacking");
        enemy.agent.ResetPath();
        enemyAnim = enemy.enemyAnim;
        enemyAnim.SetBool("IsMoving", false);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if ((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude > enemy.rangeBeforeAttack || !enemy.CheckLineOfSight(true, enemy.transform.forward, enemy.transform.position + new Vector3(0, 0.5f, 0)))
        {
            enemy.SwitchState(enemy.runState);
        }
        else
        {
            Attack();
        }
        //enemy.RotateToPosition(GameManager.instance.XROrigin.transform.position);
        enemy.rig.SetTarget(GameManager.instance.cam.transform.position);
    }

    private void Attack()
    {
        Debug.Log("Attacking player");
    }
}
