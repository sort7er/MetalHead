using UnityEngine;
using UnityEngine.AI;

public class EnemyDieState : EnemyBaseState
{
    private RunningEnemy runningEnemy;
    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state die");
        runningEnemy = enemy;
        enemy.SetGlowColor(Color.black);
        enemy.EnableRagdoll(true);
        enemy.DelayedCallback(enemy.dieState, "StartDestroy", enemy.timeDead);
        GameManager.instance.CheckWithGates();
        AIManager.instance.DoneAttacking();
        runningEnemy.voiceLines.Dying();
        runningEnemy.parryCanvas.SetActive(false);
        foreach(Transform t in enemy.transform)
        {
            t.gameObject.layer = 0;
        }
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
    public void StartDestroy()
    {
        runningEnemy.DestroyNow();
    }
}
