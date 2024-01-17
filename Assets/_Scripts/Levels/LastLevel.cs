using UnityEngine;

public class LastLevel : MonoBehaviour
{
    public EnemyHealth[] health;

    private int numEnemies;

    public void CheckEnemies()
    {

        int deadEnemies = 0;
        numEnemies = health.Length;

        for (int i = 0; i < numEnemies; i++)
        {
            if (health[i].IsDead())
            {
                deadEnemies++;
            }
        }
        if(deadEnemies >= numEnemies)
        {
            GameManager.instance.WonMenu();
        }
    }
}
