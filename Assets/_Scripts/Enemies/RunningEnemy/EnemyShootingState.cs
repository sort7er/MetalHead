using UnityEngine;

public class EnemyShootingState : EnemyBaseAttack
{
    private RunningEnemy runningEnemy;
    private Animator enemyAnim;
    bool attackStarted, cannotAttack;
    private bool waitOneFrame, waitTwoFrames, fromRunTransition, freezeRotation, lineOfSightToPlayer;
    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state attacking");
        enemy.agent.ResetPath();
        runningEnemy = enemy;
        enemyAnim = enemy.enemyAnim;
        enemyAnim.SetBool("IsMoving", false);
        waitOneFrame = false;
        waitTwoFrames = false;
        freezeRotation = false;
        fromRunTransition = false;
        enemy.DelayedCallback(enemy.attackState, "FromRunTransition", 0.1f);
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.headTrans.position, enemy.directionToPlayer, out hit, enemy.rangeBeforeAttack + 0.5f, runningEnemy.layersLookForPlayer))
        {
            if (hit.transform.tag == "Player")
            {
                RaycastHit hit2;
                if (Physics.Raycast(enemy.leftSholder.position, (GameManager.instance.XROrigin.transform.position - enemy.leftSholder.position), out hit2, enemy.rangeBeforeAttack + 2f, runningEnemy.layersLookForPlayer))
                {
                    if (hit2.transform.tag == "Player")
                    {
                        RaycastHit hit3;
                        if (Physics.Raycast(enemy.rightSholder.position, (GameManager.instance.XROrigin.transform.position - enemy.rightSholder.position), out hit3, enemy.rangeBeforeAttack + 2f, runningEnemy.layersLookForPlayer))
                        {
                            if (hit3.transform.tag == "Player")
                            {
                                lineOfSightToPlayer = true;
                            }
                            else
                            {
                                lineOfSightToPlayer = false;
                            }
                        }
                        else
                        {
                            lineOfSightToPlayer = false;
                        }
                    }
                    else
                    {
                        lineOfSightToPlayer = false;
                    }
                }
                else
                {
                    lineOfSightToPlayer = false;
                }
            }
            else
            {
                lineOfSightToPlayer = false;
            }
        }
        else
        {
            lineOfSightToPlayer = false;
        }
        if ((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude < enemy.tooCloseDistance && !attackStarted)
        {
            enemy.SwitchState(enemy.coverState);
        }
        else if (((GameManager.instance.XROrigin.transform.position - enemy.transform.position).magnitude > enemy.rangeBeforeAttack || !lineOfSightToPlayer) && !attackStarted)
        {
            enemy.SwitchState(enemy.runState);
        }
        else if (!attackStarted && !cannotAttack && fromRunTransition)
        {
            //if (AIManager.instance.CheckForAttack())
            //{

            //}
            Attack();
        }
        enemy.rig.SetTarget(GameManager.instance.cam.transform.position);

        if (!attackStarted && !freezeRotation)
        {
            enemy.RotateToPosition(GameManager.instance.XROrigin.transform.position);

        }

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

        //Switch to other states
        if (enemy.stunned)
        {
            enemy.SwitchState(enemy.stunnedState);
            AttackDone();
            enemy.weapon.CannotParry();
            enemy.weapon.NotLethal();
        }
        if (enemy.hiding && !attackStarted)
        {
            enemy.SwitchState(enemy.coverState);
            AttackDone();
            enemy.weapon.CannotParry();
            enemy.weapon.NotLethal();
        }
    }

    private void Attack()
    {
        runningEnemy.voiceLines.Attacking();
        attackStarted = true;
        cannotAttack = true;
        waitOneFrame = false;
        waitTwoFrames = false;
        //int randomNumber = Random.Range(1, 3);
        runningEnemy.ChangeAnimationState("Shoot");
    }
    private void ShootingDone()
    {
        enemyAnim.SetBool("Shooting", true);
    }
    public void AttackDone()
    {
        //AIManager.instance.DoneAttacking();
        runningEnemy.CanAttack();
        attackStarted = false;
    }
    public override void AttackCoolDown()
    {
        cannotAttack = false;
    }
    public void FromRunTransition()
    {
        fromRunTransition = true;
    }
}
