using UnityEngine;

public class EnemyKickState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state kick");
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
