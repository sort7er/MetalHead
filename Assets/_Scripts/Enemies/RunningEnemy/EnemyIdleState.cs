using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state idle");
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if (enemy.enemyDistanceCheck)
        {
            //Maybe check camera offset
            if (Vector3.Distance(enemy.transform.position, GameManager.instance.XROrigin.transform.position) <= enemy.sightRange)
            {
                if (Vector3.Angle(enemy.directionToPlayer, enemy.transform.forward) <= enemy.FOV * 0.5f)
                {
                    enemy.SwitchState(enemy.susState);
                }
            }
            enemy.DistanceCheckOff();
        }
    }
}
