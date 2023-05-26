using UnityEngine;

public abstract class EnemyBaseAttack : EnemyBaseState
{

    public override void EnterState(RunningEnemy enemy)
    {
    }

    public override void UpdateState(RunningEnemy enemy)
    {
    }
    public abstract void AttackCoolDown();

}
