using UnityEngine;
using UnityEngine.AI;

public class EnemyRunState : EnemyBaseState
{
    private float timer;

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state run");
        enemy.agent.ResetPath();
        enemy.SetSpeed(enemy.runSpeed);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= enemy.rangeBeforeAttack && enemy.CheckLineOfSight(true, enemy.directionToPlayer))
        {
            enemy.SwitchState(enemy.attackState);
            enemy.agent.ResetPath();
        }
        else
        {
            enemy.agent.SetDestination(GameManager.instance.XROrigin.transform.position);
            if (enemy.CheckLineOfSight(true, enemy.directionToPlayer))
            {
                enemy.RotateToPosition(GameManager.instance.XROrigin.transform.position);
            }
            else
            {
                enemy.RotateToPosition(enemy.transform.position + enemy.movementDircetion);
            }
        }

        if (!enemy.inView)
        {
            if(timer < enemy.timeBeforeLost)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = enemy.timeBeforeLost;
                enemy.SetPointOfInterest(GameManager.instance.XROrigin.transform.position);
                enemy.SwitchState(enemy.searchingState);
            }
            
        }
        else
        {

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
            }

        }

        enemy.LookingForPlayer(enemy.runSightRange);
    }
}
