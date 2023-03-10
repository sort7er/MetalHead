using UnityEngine;
using UnityEngine.AI;

public class EnemyDieState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state die");
        enemy.agent.enabled = false;
        enemy.rb.isKinematic = false;
        enemy.rb.useGravity = true;
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
