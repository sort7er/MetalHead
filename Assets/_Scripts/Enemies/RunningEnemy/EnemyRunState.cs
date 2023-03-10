using UnityEngine;
using UnityEngine.AI;

public class EnemyRunState : EnemyBaseState
{
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state run");
        enemy.agent.ResetPath();
        enemy.sliderFill.color = enemy.seenColor;
        enemy.sliderBackground.color = enemy.susDetectionColor;
        enemy.detectionSlider.maxValue = enemy.timeBeforeSeen;
        enemy.detectionSlider.value = enemy.timeBeforeSeen;


    }

    public override void UpdateState(RunningEnemy enemy)
    {
        enemy.agent.SetDestination(GameManager.instance.XROrigin.transform.position);
    }
}
