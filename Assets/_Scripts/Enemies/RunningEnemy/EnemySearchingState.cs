using System.Drawing;
using UnityEngine;

public class EnemySearchingState : EnemyBaseState
{
    private float timer;
    private bool targetReached, lookingDone;
    private Animator enemyAnim;
    private Vector3 targetPos, point;
    private RunningEnemy runningEnemy;

    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state searching");
        runningEnemy = enemy;
        enemyAnim = enemy.enemyAnim;
        enemyAnim.SetBool("IsMoving", true);
        enemy.SetSpeed(enemy.searchingSpeed);
        enemy.SetAnimSpeed(0f);
        enemy.SetGlowColor(enemy.searchingColor);
        enemy.agent.ResetPath();
        enemy.agent.avoidancePriority = 51;
        if (enemy.pointOfInterest != null)
        {
            enemy.SetNavMeshDestination(enemy.pointOfInterest);
            lookingDone = false;
        }
        else
        {
            lookingDone = true;
            enemy.DelayedCallback(enemy.searchingState, "NextTarget", 0,5f);
        }
        enemy.PlayerInSight(false);
        targetReached = false;
        timer = 0;
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        //Walk to target
        if (!lookingDone)
        {
            Debug.Log("-1");
            if ((new Vector3(enemy.pointOfInterest.x, enemy.transform.position.y, enemy.pointOfInterest.z) - enemy.transform.position).magnitude <= 1.5f && !targetReached)
            {
                enemy.agent.ResetPath();
                targetReached = true;
                enemyAnim.SetBool("IsMoving", false);
                enemyAnim.SetBool("LookingAround", true);
                enemy.DelayedCallback(enemy.searchingState, "DoneLookingAround", Random.Range(enemy.searchMinTimeBetweenTargets, enemy.searchMaxTimeBetweenTargets));
                Debug.Log("1");
            }

            if (Mathf.Abs(enemy.movementDircetion.magnitude) > 0.01f)
            {
                enemy.RotateToPosition(enemy.transform.position + enemy.movementDircetion);
            }
        }
        else
        {
            Debug.Log("2");
            if ((targetPos - enemy.transform.position).magnitude <= 1f && !targetReached)
            {
                enemy.agent.ResetPath();
                enemy.enemyAnim.SetBool("IsMoving", false);
                enemyAnim.SetBool("LookingAround", true);
                enemy.DelayedCallback(enemy.searchingState, "NextTarget", Random.Range(enemy.searchMinTimeBetweenTargets, enemy.searchMaxTimeBetweenTargets));
                targetReached = true;
            }
            else if (!targetReached)
            {
                Debug.Log("3");
                //Rotate to point of interest
                if (enemy.CheckLineOfSight(true, enemy.directionToPointOfInterest, enemy.headTrans.position))
                {
                    enemy.rig.SetRig(true);
                    enemy.rig.SetTarget(enemy.pointOfInterest);
                }
                else
                {
                    enemy.rig.SetRig(false);
                }

                if (Mathf.Abs(enemy.movementDircetion.magnitude) > 0.01f)
                {
                    
                    enemy.RotateToPosition(enemy.transform.position + enemy.movementDircetion);
                }
            }
        }
        

        //Looking for player
        enemy.LookingForPlayer(enemy.searchingSightRange);
        if (enemy.inView)
        {
            enemy.rig.SetRig(true);
            enemy.rig.SetTarget(GameManager.instance.cam.transform.position);
            enemy.SetDistanceCheck(0);

            if (timer >= enemy.timeBeforeSeen)
            {
                enemyAnim.SetBool("LookingAround", false);
                timer = enemy.timeBeforeSeen;
                enemy.voiceLines.SearchingRun();
                enemy.SwitchState(enemy.runState);
            }
            else
            {
                timer += Time.deltaTime;
            }
            if ((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= enemy.distanceBeforeImmediateDetection)
            {
                enemyAnim.SetBool("LookingAround", false);
                enemy.SetPointOfInterest(GameManager.instance.cam.transform.position);
                enemy.voiceLines.SearchingRun();
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
            enemyAnim.SetBool("LookingAround", false);
            enemy.SwitchState(enemy.stunnedState);
        }
        if (enemy.hiding)
        {
            enemyAnim.SetBool("LookingAround", false);
            enemy.SwitchState(enemy.coverState);
        }
    }
    public void DoneLookingAround()
    {

        lookingDone = true;
        runningEnemy.DelayedCallback(runningEnemy.searchingState, "NextTarget", 0.5f);
    }
    public void NextTarget()
    {
        if (runningEnemy.RandomPointOnNavMesh(runningEnemy.transform.position, runningEnemy.idleRandomDestinationRange, out point))
        {
            targetPos = point;
        }
        enemyAnim.SetBool("LookingAround", false);
        runningEnemy.enemyAnim.SetBool("IsMoving", true);
        targetReached = false;
        runningEnemy.SetPointOfInterest(targetPos);
        runningEnemy.DelayedCallback(runningEnemy.searchingState, "StartWalking", 0.3f);
    }
    public void StartWalking()
    {
        runningEnemy.SetNavMeshDestination(targetPos);
    }

}
