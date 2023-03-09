using UnityEngine;

public class EnemySusState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state sus");
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
