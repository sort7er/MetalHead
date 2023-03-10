using UnityEngine;

public class EnemySearchingState : EnemyBaseState
{
    private float timer;
    private bool targetReached, lookingDone;


    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state searching");
        enemy.SetSpeed(enemy.searchingSpeed);
        enemy.SetFOV(enemy.searchingFOV);
        enemy.agent.ResetPath();
        enemy.agent.SetDestination(enemy.pointOfInterest);
        enemy.sliderBackground.color = enemy.susDetectionColor;
        enemy.sliderFill.color = enemy.seenColor;
        enemy.detectionSlider.value = 0;
        enemy.detectionSlider.maxValue = enemy.timeBeforeSeen;

        //Fix this later, could probably be a problem
        targetReached = false;
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        enemy.detectionSlider.value = timer;
        enemy.LookingForPlayer(enemy.searchingSightRange);


        //Walk to target
        if ((new Vector3(enemy.pointOfInterest.x, enemy.transform.position.y, enemy.pointOfInterest.z) - enemy.transform.position).magnitude <= 1.5f && !targetReached)
        {
            enemy.agent.ResetPath();
            targetReached = true;
            LookingAround();
            enemy.DelayedCallback(enemy.searchingState, "DoneLookingAround", Random.Range(enemy.minSearchDuration, enemy.maxSearchDuration));
        }
        else if(!targetReached)
        {
            //Rotate to point of interest
            if (enemy.CheckLineOfSight(true, enemy.pointOfInterest))
            {
                enemy.RotateToPosition(enemy.pointOfInterest);
            }
            else
            {
                enemy.RotateToPosition(enemy.transform.position + enemy.movementDircetion);
            }
        }


        if (enemy.inView)
        {
            enemy.SetDistanceCheck(0);

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
        if (lookingDone)
        {
            Debug.Log("Start some walking pattern around");
        }
    }


    private void LookingAround()
    {
        Debug.Log("Looking around animation");
    }
    public void DoneLookingAround()
    {
        lookingDone = true;
    }

}
