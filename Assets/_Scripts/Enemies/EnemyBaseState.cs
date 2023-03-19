using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(RunningEnemy enemy);

    public abstract void UpdateState(RunningEnemy enemy);

}
