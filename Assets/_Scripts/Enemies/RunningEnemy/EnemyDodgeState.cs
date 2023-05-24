using UnityEngine;
using UnityEngine.AI;

public class EnemyDodgeState : EnemyBaseState
{
    private RunningEnemy runningEnemy;
    private NavMeshAgent agent;
    private Animator enemyAnim;
    private Vector3 dodgeDestination;
    private float dodgeSpeed;
    private bool canLeft, canRight;
    private int direction;

    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state dodge");
        runningEnemy = enemy;
        agent = enemy.agent;
        agent.avoidancePriority = 49;
        enemyAnim = enemy.enemyAnim;
        agent.ResetPath();
        dodgeSpeed = enemy.dodgeSpeed;
        canLeft = false;
        canRight = false;
        direction = 0;
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(runningEnemy.transform.position, out myNavHit, 1, NavMesh.AllAreas))
        {
            runningEnemy.transform.position = myNavHit.position;
        }
        Dodge();
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        //Switch to other states
        if (enemy.stunned)
        {
            runningEnemy.IsHiding(false);
            enemy.SwitchState(enemy.stunnedState);
        }
    }

    private void Dodge()
    {
        NavMeshHit navHitRight;
        if (NavMesh.SamplePosition(runningEnemy.transform.position + runningEnemy.transform.right, out navHitRight, 0.5f, NavMesh.AllAreas) && !Physics.Raycast(runningEnemy.transform.position + new Vector3(0, 0.5f, 0), runningEnemy.transform.right, 1, runningEnemy.hidebleLayer))
        {
            canRight = true;
        }
        NavMeshHit navHitLeft;
        if (NavMesh.SamplePosition(runningEnemy.transform.position - runningEnemy.transform.right, out navHitLeft, 0.5f, NavMesh.AllAreas) && !Physics.Raycast(runningEnemy.transform.position + new Vector3(0, 0.5f, 0), -runningEnemy.transform.right, 1, runningEnemy.hidebleLayer))
        {
            canLeft = true;
        }
        if (!canLeft && !canRight)
        {
            HidingDone();
        }
        else
        {
            if (canLeft && canRight)
            {
                direction = Random.Range(0, 2);
            }
            else if (!canLeft && canRight)
            {
                direction = 0;
            }
            else if (canLeft && !canRight)
            {
                direction = 1;
            }

            if (direction == 0)
            {
                runningEnemy.ChangeAnimationState("DodgeRight");
                dodgeDestination = runningEnemy.transform.position + runningEnemy.transform.right;

            }
            else if (direction == 1)
            {
                runningEnemy.ChangeAnimationState("DodgeLeft");
                dodgeDestination = runningEnemy.transform.position - runningEnemy.transform.right;
            }
            agent.speed = dodgeSpeed;
            runningEnemy.agent.SetDestination(dodgeDestination);
            Debug.Log("1");
            runningEnemy.DelayedCallback(runningEnemy.dodgeState, "StartDodge", 0.5f);
        }
    }
    public void StartDodge()
    {
        Debug.Log("2");
        agent.ResetPath();
        runningEnemy.DelayedCallback(runningEnemy.dodgeState, "HidingDone", 0.5f);
        enemyAnim.SetBool("IsMoving", false);
    }
    public void HidingDone()
    {
        Debug.Log("3");
        runningEnemy.IsHiding(false);
        if (runningEnemy.playerInSight)
        {
            runningEnemy.SwitchState(runningEnemy.runState);

        }
        else
        {
            runningEnemy.SwitchState(runningEnemy.searchingState);
        }
    }
}
