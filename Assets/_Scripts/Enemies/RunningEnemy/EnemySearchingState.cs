using UnityEngine;

public class EnemySearchingState : EnemyBaseState
{
    private Quaternion targetAngle;
    private Vector3 pointOfInterest;
    public override void EnterState(RunningEnemy enemy)
    {
        Debug.Log("Entered state searching");
        enemy.agent.ResetPath();
        pointOfInterest = enemy.pointOfInterest;
    }

    public override void UpdateState(RunningEnemy enemy)
    {
        targetAngle = Quaternion.LookRotation(new Vector3(enemy.pointOfInterest.x, enemy.transform.position.y, enemy.pointOfInterest.z) - enemy.transform.position, enemy.transform.up);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetAngle, Time.deltaTime * enemy.turnSmoothTime);
    }
}
