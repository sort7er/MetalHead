using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state idle");
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
