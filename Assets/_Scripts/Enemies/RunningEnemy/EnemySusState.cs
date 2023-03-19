using UnityEngine;
using UnityEngine.AI;

public class EnemySusState : EnemyBaseState
{
    private RunningEnemy runningEnemy;
    private NavMeshAgent agent;
    private Animator enemyAnim;
    private Vector3 pointOfInterest;
    private bool targetReached, startRotate;
    private float sightRange, timer;

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state sus");
        agent = enemy.agent;
        enemyAnim = enemy.enemyAnim;
        runningEnemy = enemy;
        enemy.SetSpeed(enemy.susSpeed);
        enemy.SetGlowColor(enemy.susColor);
        pointOfInterest = enemy.pointOfInterest;
        targetReached = false;
        startRotate = false;
        sightRange = enemy.susSightRange;
        agent.ResetPath();
        enemyAnim.SetBool("IsMoving", false);
        enemy.SetAnimSpeed(0f);
        enemy.DelayedCallback(enemy.susState, "CheckItOut", 2);
        enemy.rig.SetRig(true);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        //Walk to target
        if ((new Vector3(pointOfInterest.x, enemy.transform.position.y, pointOfInterest.z) - enemy.transform.position).magnitude <= 0.5f && !targetReached)
        {
            agent.ResetPath();
            targetReached = true;
            enemyAnim.SetBool("IsMoving", false);
            enemyAnim.SetBool("LookingAround", true);
            enemy.rig.SetRig(false);
            enemy.DelayedCallback(enemy.susState, "DoneLookingAround", Random.Range(enemy.maxSusDuration, enemy.maxSusDuration));
        }
        else
        {
            enemy.rig.SetTarget(pointOfInterest);
            if (startRotate)
            {
                enemy.RotateToPosition(pointOfInterest);
            }
        }
        //Looking for player
        enemy.LookingForPlayer(sightRange);
        if (enemy.inView)
        {
            enemy.SetDistanceCheck(0);

            enemy.rig.SetRig(true);
            enemy.rig.SetTarget(GameManager.instance.cam.transform.position);
            if (timer >= enemy.timeBeforeDetect)
            {
                timer = enemy.timeBeforeDetect;
                enemy.PlayerDetected();
                enemyAnim.SetBool("LookingAround", false);
                enemy.SwitchState(enemy.runState);

            }
            else
            {
                timer += Time.deltaTime;
            }
            if ((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude <= enemy.distanceBeforeImmediateDetection)
            {
                enemy.SetPointOfInterest(GameManager.instance.cam.transform.position);
                enemyAnim.SetBool("LookingAround", false);
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
    public void CheckItOut()
    {
        if (!targetReached)
        {
            startRotate = true;
            enemyAnim.SetBool("IsMoving", true);
            runningEnemy.SetNavMeshDestination(pointOfInterest);
        }
    }
    public void DoneLookingAround()
    {
        enemyAnim.SetBool("LookingAround", false);
        runningEnemy.SwitchState(runningEnemy.idleState);
    }
}
