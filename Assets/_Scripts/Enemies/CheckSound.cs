using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CheckSound : MonoBehaviour
{
    public static CheckSound instance;

    private void Awake()
    {
        instance = this;
    }

    public LayerMask enemyMask;
    private Collider[] possibleEnemiesWhoHeardMe;
    //Add more later
    private RunningEnemy runningEnemy;

    public void CheckIfEnemyCanHearTheSound(Vector3 soundSource, float range, bool justSus)
    {
        possibleEnemiesWhoHeardMe = Physics.OverlapSphere(soundSource, range, enemyMask);

        foreach (Collider enemy in possibleEnemiesWhoHeardMe)
        {
            if (enemy.GetComponentInParent <RunningEnemy>() != null && enemy.GetComponent<RigBuilder>() != null)
            {
                runningEnemy = enemy.GetComponentInParent<RunningEnemy>();
                if (justSus)
                {
                    runningEnemy.EnemySus(soundSource);
                }
                else
                {
                    runningEnemy.EnemyAlert(soundSource);
                }
            }
        }
    }
}
