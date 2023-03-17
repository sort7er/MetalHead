using UnityEngine;

public class EnemySearchingState : EnemyBaseState
{
    private float timer;
    private bool targetReached, lookingDone;


    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state searching");
        enemy.SetSpeed(enemy.searchingSpeed);
        enemy.SetGlowColor(enemy.searchingColor);
        enemy.agent.ResetPath();
        enemy.agent.SetDestination(enemy.pointOfInterest);
        enemy.PlayerInSight(false);
        //Fix this later, could probably be a problem
        targetReached = false;
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        //Walk to target
        if ((new Vector3(enemy.pointOfInterest.x, enemy.transform.position.y, enemy.pointOfInterest.z) - enemy.transform.position).magnitude <= 1.5f && !targetReached)
        {
            enemy.agent.ResetPath();
            targetReached = true;
            LookingAround();
            enemy.DelayedCallback(enemy.searchingState, "DoneLookingAround", Random.Range(enemy.minSearchDuration, enemy.maxSearchDuration));
        }

        //Rotate to point of interest
        if (enemy.CheckLineOfSight(false, enemy.directionToPointOfInterest))
        {
            enemy.RotateToPosition(enemy.pointOfInterest);
        }
        else if (Mathf.Abs(enemy.movementDircetion.magnitude) > 0.01f)
        {
            enemy.RotateToPosition(enemy.transform.position + enemy.movementDircetion);
        }

        //Looking for player
        enemy.LookingForPlayer(enemy.searchingSightRange);
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
            if ((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= enemy.distanceBeforeImmediateDetection)
            {
                enemy.SetPointOfInterest(GameManager.instance.XROrigin.transform.position);
                enemy.SwitchState(enemy.runState);
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
