using UnityEngine;
using UnityEngine.AI;

public class EnemyDieState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state die");
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
