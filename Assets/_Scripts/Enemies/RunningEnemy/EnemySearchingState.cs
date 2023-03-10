using UnityEngine;

public class EnemySearchingState : EnemyBaseState
{
    private Quaternion targetAngle;
    private float sightRange, timer;

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state searching");
        enemy.SetSpeed(enemy.searchingSpeed);
        enemy.agent.ResetPath();
        enemy.sliderBackground.color = enemy.susDetectionColor;
        enemy.sliderFill.color = enemy.seenColor;
        enemy.detectionSlider.value = 0;
        enemy.detectionSlider.maxValue = enemy.timeBeforeSeen;
        sightRange = enemy.searchingSightRange;
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        enemy.RotateToPosition(enemy.pointOfInterest);

        enemy.detectionSlider.value = timer;

        //Looking for player
        enemy.LookingForPlayer(sightRange);

        if (enemy.inView)
        {
            enemy.SetDistanceCheck(0);
            Debug.Log("lol");

            if (timer >= enemy.timeBeforeSeen)
            {
                timer = enemy.timeBeforeSeen;
                enemy.SwitchState(enemy.runState);
            }
            else
            {
                timer += Time.deltaTime;
            }

        }
        else
        {
            Debug.Log("3");
            enemy.SetDistanceCheck(enemy.defaultTimeBetweenDistanceCheck);
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
        }
    }

}
