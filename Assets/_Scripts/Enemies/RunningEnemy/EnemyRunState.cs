using UnityEngine;
using UnityEngine.AI;

public class EnemyRunState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state run");
        enemy.agent.ResetPath();
        enemy.SetSpeed(enemy.runSpeed);
        enemy.sliderFill.color = enemy.seenColor;
        enemy.sliderBackground.color = enemy.susDetectionColor;
        enemy.detectionSlider.maxValue = enemy.timeBeforeSeen;
        enemy.detectionSlider.value = enemy.timeBeforeSeen;


    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= enemy.rangeBeforeAttack && enemy.CheckLineOfSight(true))
        {
            enemy.SwitchState(enemy.attackState);
            enemy.agent.ResetPath();
        }
        else
        {
            enemy.agent.SetDestination(GameManager.instance.XROrigin.transform.position);
        }
        enemy.RotateToPosition(GameManager.instance.XROrigin.transform.position);
    }
}
