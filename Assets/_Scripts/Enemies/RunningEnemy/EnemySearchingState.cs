using UnityEngine;

public class EnemySearchingState : EnemyBaseState
{
    private float timer;
    private bool targetReached, lookingDone;
    private Animator enemyAnim;

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state searching");
        enemyAnim = enemy.enemyAnim;
        enemyAnim.SetBool("IsMoving", true);
        enemy.SetSpeed(enemy.searchingSpeed);
        enemy.SetAnimSpeed(0.25f);
        enemy.SetGlowColor(enemy.searchingColor);
        enemy.agent.ResetPath();
        enemy.SetNavMeshDestination(enemy.pointOfInterest);
        enemy.PlayerInSight(false);
        //Fix this later, could probably be a problem
        targetReached = false;
        timer = 0;
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        //Walk to target
        if ((new Vector3(enemy.pointOfInterest.x, enemy.transform.position.y, enemy.pointOfInterest.z) - enemy.transform.position).magnitude <= 0.2f && !targetReached)
        {
            enemy.agent.ResetPath();
            targetReached = true;
            enemyAnim.SetBool("IsMoving", false);
            enemyAnim.SetBool("LookingAround", true);
            enemy.DelayedCallback(enemy.searchingState, "DoneLookingAround", Random.Range(enemy.minSearchDuration, enemy.maxSearchDuration));
        }

        //Rotate to point of interest
        if (enemy.CheckLineOfSight(true, enemy.directionToPointOfInterest, enemy.headTrans.position))
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
            enemy.rig.SetRig(true);
            enemy.rig.SetTarget(GameManager.instance.cam.transform.position);
            enemy.SetDistanceCheck(0);

            if (timer >= enemy.timeBeforeSeen)
            {
                enemyAnim.SetBool("LookingAround", false);
                timer = enemy.timeBeforeSeen;
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
        if (lookingDone)
        {
            Debug.Log("Start some walking pattern around");
        }
    }
    public void DoneLookingAround()
    {
        lookingDone = true;
    }

}
