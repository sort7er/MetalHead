using UnityEngine;

public class EnemyStunnedState : EnemyBaseState
{
    private bool waitOneFrame, waitTwoFrames;
    private RunningEnemy runningEnemy;
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state stunned");
        runningEnemy = enemy;
        enemy.agent.ResetPath();
        enemy.rig.SetRig(false);
        enemy.ChangeAnimationState("Stunned" + enemy.currentBodyPart);
        waitOneFrame = false;
        waitTwoFrames = false;
        runningEnemy.enemyAnim.SetBool("LookingAround", false);
        runningEnemy.enemyAnim.SetBool("InCover", false);
        runningEnemy.enemyAnim.SetBool("IsMoving", false);


    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if (!waitOneFrame && !waitTwoFrames)
        {
            waitOneFrame = true;
        }
        else if (waitOneFrame && !waitTwoFrames)
        {
            waitTwoFrames = true;
            enemy.DelayedCallback(enemy.stunnedState, "StunDone", enemy.enemyAnim.GetCurrentAnimatorStateInfo(0).length - 0.25f);
        }
        
    }
    public void StunDone()
    {
        runningEnemy.IsStunned(false);
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
