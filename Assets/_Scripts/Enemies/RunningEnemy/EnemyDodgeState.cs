using UnityEngine;

public class EnemyDodgeState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state dodging");
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
