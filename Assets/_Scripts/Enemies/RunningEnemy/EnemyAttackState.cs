using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state attacking");
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        
    }
}
