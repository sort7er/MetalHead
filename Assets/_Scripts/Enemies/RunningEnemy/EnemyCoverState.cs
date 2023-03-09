using UnityEngine;

public class EnemyCoverState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state cover");
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
