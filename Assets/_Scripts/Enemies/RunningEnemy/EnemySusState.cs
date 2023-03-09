using UnityEngine;
using UnityEngine.AI;

public class EnemySusState : EnemyBaseState
{
    private NavMeshAgent agent;
    private Quaternion targetAngle;
    private Vector3 pointOfInterest;
    private bool targetReached;

    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state sus");
        agent = enemy.agent;
        enemy.SetSpeed(enemy.susSpeed);
        pointOfInterest = enemy.pointOfInterest;
        targetReached = false;
        agent.ResetPath();
        enemy.SusStart();
        
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        //Walk to target
        if ((pointOfInterest - enemy.transform.position).magnitude <= 1f && !targetReached)
        {
            agent.ResetPath();
            targetReached = true;
            Debug.Log("Yeah");
        }
        else
        {
            targetAngle = Quaternion.LookRotation(new Vector3(enemy.pointOfInterest.x, enemy.transform.position.y, enemy.pointOfInterest.z) - enemy.transform.position, enemy.transform.up);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetAngle, Time.deltaTime * enemy.turnSmoothTime);
        }

    }
    public void CheckItOut()
    {
        agent.SetDestination(pointOfInterest);
    }
}
