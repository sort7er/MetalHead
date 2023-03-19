using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBaseState
{
    private Vector3 targetPos;
    private RunningEnemy runningEnemy;
    private int currentTarget;
    private bool targetReached, nextTarget;
    private float timer;


    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state idle");
        runningEnemy = enemy;
        enemy.SetSpeed(enemy.idleSpeed);
        enemy.SetGlowColor(enemy.idleColor);
        enemy.SetAnimSpeed(0.25f);
        targetReached = false;
        if (targetPos != null)
        {
            currentTarget = Random.Range(0, enemy.tempIdleTargets.Length);
            targetPos = enemy.tempIdleTargets[currentTarget].position;
        }
        enemy.DelayedCallback(enemy.idleState, "NextTarget", 0.5f);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        //Walk to target
        if ((targetPos - enemy.transform.position).magnitude <= 1f && !targetReached)
        {
            nextTarget = false;
            enemy.agent.ResetPath();
            enemy.enemyAnim.SetBool("IsMoving", false);
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
            enemy.rig.SetRig(true);
            enemy.rig.SetTarget(GameManager.instance.cam.transform.position);
            enemy.SetDistanceCheck(0);
            if (timer >= enemy.timeBeforeSus)
            {
                timer = enemy.timeBeforeSus;
                enemy.SetPointOfInterest(GameManager.instance.cam.transform.position);
                enemy.SwitchState(enemy.susState);
                
            }
            else
            {
                timer += Time.deltaTime;
            }

            if((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= enemy.distanceBeforeImmediateDetection)
            {
                enemy.SetPointOfInterest(GameManager.instance.cam.transform.position);
                enemy.SwitchState(enemy.runState);
            }

        }
        else
        {
            enemy.rig.SetRig(false);
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
        runningEnemy.enemyAnim.SetBool("IsMoving", true);
        nextTarget = true;
        runningEnemy.SetNavMeshDestination(targetPos);
        targetReached = false;
    }
}
    