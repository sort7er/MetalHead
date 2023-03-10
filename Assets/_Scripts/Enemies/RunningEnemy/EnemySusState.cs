using UnityEngine;
using UnityEngine.AI;

public class EnemySusState : EnemyBaseState
{
    private NavMeshAgent agent;
    private Vector3 pointOfInterest;
    private Quaternion targetAngle;
    private bool targetReached, susDone;
    private float sightRange, timer;

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state sus");
        agent = enemy.agent;
        enemy.SetSpeed(enemy.susSpeed);
        enemy.sliderBackground.color = enemy.idleDetectionColor;
        enemy.sliderFill.color = enemy.susDetectionColor;
        enemy.detectionSlider.value = 0;
        pointOfInterest = enemy.pointOfInterest;
        targetReached = false;
        susDone = false;
        sightRange = enemy.susSightRange;
        agent.ResetPath();
        enemy.SusStart();
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        enemy.detectionSlider.value = timer;
        //Walk to target
        if ((new Vector3(pointOfInterest.x, enemy.transform.position.y, pointOfInterest.z) - enemy.transform.position).magnitude <= 1f && !targetReached)
        {
            agent.ResetPath();
            targetReached = true;
            LookingAround();
            enemy.LookingAround();
        }
        else
        {
            targetAngle = Quaternion.LookRotation(new Vector3(enemy.pointOfInterest.x, enemy.transform.position.y, enemy.pointOfInterest.z) - enemy.transform.position, enemy.transform.up);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetAngle, Time.deltaTime * enemy.turnSmoothTime);
        }
        //Looking for player
        enemy.LookingForPlayer(sightRange);
        if (enemy.inView)
        {
            enemy.SetDistanceCheck(0);


            if (timer >= enemy.timeBeforeDetect)
            {
                timer = enemy.timeBeforeDetect;
                enemy.SetDistanceCheck(enemy.defaultTimeBetweenDistanceCheck);
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
        if (susDone)
        {
            enemy.SwitchState(enemy.idleState);
        }

    }
    public void CheckItOut()
    {
        if (!targetReached)
        {
            agent.SetDestination(pointOfInterest);
        }
    }
    private void LookingAround()
    {
        Debug.Log("Looking around animation");
    }
    public void DoneLookingAround()
    {
        susDone = true;
    }
}
