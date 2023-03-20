using UnityEngine;
using UnityEngine.AI;

public class EnemyDieState : EnemyBaseState
{
    private RunningEnemy runningEnemy;
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state die");
        runningEnemy = enemy;
        enemy.SetGlowColor(Color.black);
        enemy.EnableRagdoll(true);
        enemy.DelayedCallback(enemy.dieState, "StartDestroy", enemy.timeDead);
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
    public void StartDestroy()
    {
        runningEnemy.DestroyNow();
    }
}
