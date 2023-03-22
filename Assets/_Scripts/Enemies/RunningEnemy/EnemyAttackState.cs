using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private RunningEnemy runningEnemy;
    private Animator enemyAnim;
    bool attackStarted, cannotAttack;
    private bool waitOneFrame, waitTwoFrames, fromRunTransition;
    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state attacking");
        enemy.agent.ResetPath();
        runningEnemy = enemy;
        enemyAnim = enemy.enemyAnim;
        enemyAnim.SetBool("IsMoving", false);
        waitOneFrame = false;
        waitTwoFrames = false;
        fromRunTransition = false;
        enemy.DelayedCallback(enemy.attackState, "FromRunTransition", 0.1f);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        if (((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude > enemy.rangeBeforeAttack || !enemy.CheckLineOfSight(true, enemy.transform.forward, enemy.transform.position + new Vector3(0, 0.5f, 0))) && !attackStarted)
        {
            enemy.SwitchState(enemy.runState);
        }
        else if(!attackStarted && !cannotAttack && fromRunTransition)
        {
            if (AIManager.instance.CheckForAttack())
            {
                Attack();
            }            
        }
        enemy.rig.SetTarget(GameManager.instance.cam.transform.position);


        //Wait to get animation length
        if (attackStarted)
        {
            if (!waitOneFrame && !waitTwoFrames)
            {
                waitOneFrame = true;
            }
            else if (waitOneFrame && !waitTwoFrames)
            {
                waitTwoFrames = true;
                enemy.DelayedCallback(enemy.attackState, "AttackDone", enemyAnim.GetCurrentAnimatorStateInfo(0).length);
            }
        }

        //Parryed
        if (enemy.weapon.isParrying)
        {
            enemy.enemyAnim.SetTrigger("Parry");
            enemy.weapon.ParryingDone();
        }
        else
        {
            enemy.enemyAnim.ResetTrigger("Parry");
        }


        //Switch to other states
        if (enemy.stunned)
        {
            enemy.SwitchState(enemy.stunnedState);
            AIManager.instance.DoneAttacking();
            enemy.weapon.CannotParry();
            enemy.weapon.NotLethal();
        }
        if (enemy.hiding && !attackStarted)
        {
            enemy.SwitchState(enemy.coverState);
            AIManager.instance.DoneAttacking();
            enemy.weapon.CannotParry();
            enemy.weapon.NotLethal();
        }
    }

    private void Attack()
    {
        attackStarted = true;
        cannotAttack = true;
        waitOneFrame = false;
        waitTwoFrames = false;
        int randomNumber = Random.Range(1, 3);
        runningEnemy.ChangeAnimationState("Attack" + randomNumber.ToString());
    }
    public void AttackDone()
    {
        AIManager.instance.DoneAttacking();
        runningEnemy.CanAttack();
        attackStarted = false;
    }
    public void AttackCoolDown()
    {
        cannotAttack = false;
    }
    public void FromRunTransition()
    {
        fromRunTransition = true;
    }
}
