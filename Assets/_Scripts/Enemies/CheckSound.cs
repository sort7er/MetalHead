using UnityEngine;

public class CheckSound : MonoBehaviour
{
    public static CheckSound instance;

    private void Awake()
    {
        instance = this;
    }

    public LayerMask enemyMask;
    private Collider[] possibleEnemiesWhoHeardMe;

    public void CheckIfEnemyCanHearTheSound(Vector3 soundSource, float range, bool justSus)
    {
        possibleEnemiesWhoHeardMe = Physics.OverlapSphere(soundSource, range, enemyMask);

        foreach (Collider enemy in possibleEnemiesWhoHeardMe)
        {
            if (enemy.GetComponent<RunningEnemy>() != null)
            {
                if (justSus)
                {
                    enemy.GetComponent<RunningEnemy>().EnemySus(soundSource);
                }
                else
                {
                    enemy.GetComponent<RunningEnemy>().EnemyAlert(soundSource);
                }
            }
        }
    }
}
