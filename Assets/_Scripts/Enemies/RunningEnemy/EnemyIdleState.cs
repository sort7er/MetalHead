using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBaseState
{
    private Transform enemyTrans;
    private NavMeshAgent agent;
    private Vector3 targetPos;
    private int currentTarget;
    private bool targetReached, nextTarget;
    private float timer;


    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state idle");
        enemy.SetSpeed(enemy.idleSpeed);
        enemy.sliderBackground.color = enemy.idleDefaultColor;
        enemy.sliderFill.color = enemy.idleDetectionColor;
        enemy.detectionSlider.value = 0f;
        enemy.detectionSlider.maxValue = enemy.timeBeforeSus;
        agent = enemy.agent;
        if(targetPos != null)
        {
            currentTarget = Random.Range(0, enemy.tempIdleTargets.Length);
            targetPos = enemy.tempIdleTargets[currentTarget].position;
        }
        NextTarget();
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        enemyTrans = enemy.transform;
        enemy.detectionSlider.value = timer;

        //Walk to target
        if ((targetPos - enemy.transform.position).magnitude <= 1f && !targetReached)
        {
            nextTarget = false;
            agent.ResetPath();
            enemy.DelayedCallback(enemy.idleState, "NextTarget", Random.Range(enemy.minTimeBetweenTargets, enemy.maxTimeBetweenTargets));
            targetPos = enemy.tempIdleTargets[currentTarget].position;
            targetReached = true;

            if (currentTarget >= enemy.tempIdleTargets.Length - 1)
            {
                currentTarget = 0;
            }
            else
            {
                currentTarget++;
            }
        }
        else if(nextTarget)
        {
            enemy.RotateToPosition(targetPos);
        }
        //Looking for player
        enemy.LookingForPlayer(enemy.idleSightRange);

        if (enemy.inView)
        {
            enemy.SetDistanceCheck(0);
            if (timer >= enemy.timeBeforeSus)
            {
                timer = enemy.timeBeforeSus;
                enemy.SetPointOfInterest(GameManager.instance.XROrigin.transform.position);
                enemy.SwitchState(enemy.susState);
                
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
    }
    public void NextTarget()
    {
        nextTarget = true;
        agent.SetDestination(targetPos);
        targetReached = false;
    }
}
    