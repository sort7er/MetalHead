using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBaseState
{
    private Vector3 targetPos, point;
    private RunningEnemy runningEnemy;
    private bool targetReached;
    private float timer;


    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state idle");
        enemy.agent.avoidancePriority = 52;
        runningEnemy = enemy;
        enemy.SetSpeed(enemy.idleSpeed);
        enemy.SetGlowColor(enemy.idleColor);
        enemy.SetAnimSpeed(0f);
        targetReached = false;
        if (targetPos != null)
        {
            enemy.DelayedCallback(enemy.idleState, "NextTarget", 0.5f);
        }
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        //Walk to target
        if ((targetPos - enemy.transform.position).magnitude <= 1f && !targetReached)
        {
            enemy.agent.ResetPath();
            enemy.enemyAnim.SetBool("IsMoving", false);
            enemy.DelayedCallback(enemy.idleState, "NextTarget", Random.Range(enemy.idleMinTimeBetweenTargets, enemy.idleMinTimeBetweenTargets));
            targetReached = true;
        }
        else if(!targetReached)
        {
            //Rotate to point of interest
            if (enemy.CheckLineOfSight(true, enemy.directionToPointOfInterest, enemy.headTrans.position))
            {
                enemy.RotateToPosition(enemy.pointOfInterest);
            }
            else if (Mathf.Abs(enemy.movementDircetion.magnitude) > 0.02f)
            {
                enemy.RotateToPosition(enemy.transform.position + enemy.movementDircetion);
            }
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
                enemy.voiceLines.IdleSus();
                enemy.SwitchState(enemy.susState);
                
            }
            else
            {
                timer += Time.deltaTime;
            }

            if((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= enemy.distanceBeforeImmediateDetection)
            {
                enemy.SetPointOfInterest(GameManager.instance.cam.transform.position);
                enemy.voiceLines.IdleRun();
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
        //Switch to other states
        if (enemy.stunned)
        {
            enemy.SwitchState(enemy.stunnedState);
        }
        if (enemy.hiding)
        {
            enemy.SwitchState(enemy.coverState);
        }
    }
    public void NextTarget()
    {
        if (runningEnemy.RandomPointOnNavMesh(runningEnemy.transform.position, runningEnemy.idleRandomDestinationRange, out point))
        {
            targetPos = point;
        }
        else
        {
            Debug.Log("Nah");
        }
        runningEnemy.voiceLines.Idle();
        runningEnemy.enemyAnim.SetBool("IsMoving", true);
        runningEnemy.SetNavMeshDestination(targetPos);
        runningEnemy.SetPointOfInterest(targetPos);
        targetReached = false;
    }
}
    