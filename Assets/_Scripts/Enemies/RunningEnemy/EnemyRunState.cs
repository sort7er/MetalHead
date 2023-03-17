using UnityEngine;
using UnityEngine.AI;

public class EnemyRunState : EnemyBaseState
{
    private float timer;

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state run");
        enemy.agent.ResetPath();
        enemy.SetGlowColor(enemy.detectedColor);
        enemy.SetSpeed(enemy.runSpeed);
        enemy.PlayerInSight(true);
        enemy.PlayerDetected();
        timer = 0;
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= enemy.rangeBeforeAttack && enemy.CheckLineOfSight(false, enemy.directionToPlayer))
        {
            enemy.SwitchState(enemy.attackState);
            enemy.agent.ResetPath();
        }
        else
        {
            enemy.agent.SetDestination(GameManager.instance.XROrigin.transform.position);
            if (enemy.CheckLineOfSight(false, enemy.directionToPlayer))
            {
                enemy.RotateToPosition(GameManager.instance.XROrigin.transform.position);
            }
            else if(Mathf.Abs(enemy.movementDircetion.magnitude) > 0.01f)
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
