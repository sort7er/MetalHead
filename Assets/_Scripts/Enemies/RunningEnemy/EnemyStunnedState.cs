using UnityEngine;

public class EnemyStunnedState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state stunned");
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
