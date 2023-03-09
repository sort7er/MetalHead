using UnityEngine;
using UnityEngine.AI;

public class EnemyRunState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state run");
    }

    public override void UpdateState(RunningEnemy enemy)
    {

    }
}
