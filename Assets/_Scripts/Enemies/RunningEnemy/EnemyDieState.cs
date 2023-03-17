using UnityEngine;
using UnityEngine.AI;

public class EnemyDieState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state die");
        enemy.SetGlowColor(Color.black);
        enemy.EnableRagdoll(true);
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
