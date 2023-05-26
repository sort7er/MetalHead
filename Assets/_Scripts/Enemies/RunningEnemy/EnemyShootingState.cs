using UnityEngine;
using UnityEngine.AI;

public class EnemyShootingState : EnemyBaseAttack
{
    private RunningEnemy runningEnemy;
    private Animator enemyAnim;
    private Kickable currentKickable;
    private Collider[] colliders = new Collider[3];
    bool attackStarted, cannotAttack;
    private bool waitOneFrame, waitTwoFrames, fromRunTransition, freezeRotation, lineOfSightToPlayer, loopAttack, callbackGiven, scanning;
    public override void EnterState(RunningEnemy enemy)
    {
        //Debug.Log("Entered state attacking");
        enemy.agent.ResetPath();
        runningEnemy = enemy;
        enemyAnim = enemy.enemyAnim;
        enemyAnim.SetBool("IsMoving", false);
        enemyAnim.SetBool("Shooting", false);
        loopAttack = false;
        freezeRotation = false;
        fromRunTransition = false;
        loopAttack= false;
        scanning = false;
        currentKickable = null;
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

        if (!scanning && cannotAttack && !attackStarted)
        {
            ScanArea();
            enemy.DelayedCallback(enemy.attackState, "ScanningDone", enemy.timeBetweenScan);
        }

        
        if (attackStarted && !loopAttack)//Bomb
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
        else if(attackStarted && loopAttack)//Gun
        {
            if (!lineOfSightToPlayer && !callbackGiven)
            {
                enemy.DelayedCallback(enemy.attackState, "AttackDone", enemyAnim.GetCurrentAnimatorStateInfo(0).length);
                callbackGiven = true;
            }
            else if (!runningEnemy.armRig.CanAim() && !callbackGiven)
            {
                AttackDone();
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
        
        int randomNumber = Random.Range(1, 3);
        if (randomNumber == 1 && AIManager.instance.CheckForBomb())
        {
            waitOneFrame = false;
            waitTwoFrames = false;
            runningEnemy.ChangeAnimationState("Bomb");
        }
        else
        {
            loopAttack = true;
            callbackGiven= false;
            enemyAnim.SetBool("Shooting", true);
            runningEnemy.ChangeAnimationState("Shoot");
            runningEnemy.DelayedCallback(runningEnemy.attackState, "AttackDone", Random.Range(runningEnemy.minShootingLength, runningEnemy.maxShootingLength));
        }
    }

    public void AttackDone()
    {
        //AIManager.instance.DoneAttacking();
        enemyAnim.SetBool("Shooting", false);
        runningEnemy.CanAttack();
        loopAttack = false;
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

    private void ScanArea()
    {
        scanning = true;
        if (!runningEnemy.justKicked)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i] = null;
            }

            int hits = Physics.OverlapSphereNonAlloc(runningEnemy.transform.position, runningEnemy.scanRadius, colliders, runningEnemy.scanableLayer);


            int hitReduction = 0;
            for (int i = 0; i < hits; i++)
            {
                if (colliders[i].GetComponent<Kickable>() == null)
                {
                    colliders[i] = null;
                    hitReduction++;
                }
            }
            hits -= hitReduction;

            System.Array.Sort(colliders, ColliderArraySortComparer);

            for (int i = 0; i < hits; i++)
            {
                if (!colliders[i].GetComponent<Kickable>().isBeeingKicked)
                {
                    if ((GameManager.instance.XROrigin.transform.position - runningEnemy.transform.position).magnitude > (colliders[i].transform.position - runningEnemy.transform.position).magnitude)
                    {
                        if (Vector3.Angle(colliders[i].transform.position - runningEnemy.transform.position, runningEnemy.transform.forward) <= runningEnemy.FOV * 0.5f)
                        {
                            Vector3 pointOnNavMesh;
                            NavMeshHit myNavHit;
                            if (NavMesh.SamplePosition(colliders[i].transform.position, out myNavHit, 10, NavMesh.AllAreas))
                            {
                                pointOnNavMesh = myNavHit.position;
                                NavMeshHit hit;
                                if (NavMesh.FindClosestEdge(pointOnNavMesh, out hit, NavMesh.AllAreas))
                                {
                                    if ((hit.position - colliders[i].transform.position).magnitude >= runningEnemy.barrelDistance)
                                    {
                                        enemyAnim.SetBool("Shooting", false);
                                        runningEnemy.CanAttack();
                                        loopAttack = false;
                                        attackStarted = false;
                                        cannotAttack= false;
                                        currentKickable = colliders[i].GetComponent<Kickable>();
                                        currentKickable.IsBeeingKicked(true);
                                        runningEnemy.SetKickable(currentKickable);
                                        runningEnemy.SwitchState(runningEnemy.kickState);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
    public void ScanningDone()
    {
        scanning = false;
    }
    private int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(runningEnemy.transform.position, A.transform.position).CompareTo(Vector3.Distance(runningEnemy.transform.position, B.transform.position));
        }
    }
}
