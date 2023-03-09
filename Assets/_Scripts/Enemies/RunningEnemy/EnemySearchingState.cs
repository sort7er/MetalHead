using UnityEngine;

public class EnemySearchingState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state searching");
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
