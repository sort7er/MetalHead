using UnityEngine;
using UnityEngine.AI;

public class EnemySusState : EnemyBaseState
{
    private NavMeshAgent agent;
    private Vector3 pointOfInterest;
    private Quaternion targetAngle;
    private bool targetReached, sawEnemy;
    private float sightRange, timer;

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state sus");
        agent = enemy.agent;
        sawEnemy = false;
        enemy.SetSpeed(enemy.susSpeed);
        enemy.sliderBackground.color = enemy.idleDetectionColor;
        enemy.sliderFill.color = enemy.susDetectionColor;
        enemy.detectionSlider.value = 0;
        pointOfInterest = enemy.pointOfInterest;
        targetReached = false;
        sightRange = enemy.susSightRange;
        agent.ResetPath();
        enemy.SusStart();
        
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        enemy.detectionSlider.value = timer;

        //Walk to target
        if ((pointOfInterest - enemy.transform.position).magnitude <= 1f && !targetReached)
        {
            agent.ResetPath();
            targetReached = true;
            Debug.Log("Yeah");
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
            if (!sawEnemy)
            {
                agent.ResetPath();
                sawEnemy = true;
            }


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
            if (sawEnemy)
            {
                CheckItOut();
                sawEnemy = false;
            }
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
    public void CheckItOut()
    {
        if (!targetReached)
        {
            agent.SetDestination(pointOfInterest);
        }
    }
}
