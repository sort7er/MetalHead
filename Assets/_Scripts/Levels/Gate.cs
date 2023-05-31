using TMPro;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public EnemyHealth[] enemiesInArea;
    public TextMeshProUGUI enemiesKilledText;

    private Animator gateAnim;

    private int numberOfDeadEnemies;


    private void Start()
    {
        gateAnim = GetComponent<Animator>();

        if (enemiesInArea.Length > 0)
        {
            enemiesKilledText.text = "0 / " + enemiesInArea.Length;
        }
        else
        {
            enemiesKilledText.text = "0 / 0";
        }
    }



    public void CheckIfDead()
    {
        if(enemiesInArea.Length > 0)
        {
            numberOfDeadEnemies = 0;
            for (int i = 0; i < enemiesInArea.Length; i++)
            {
                if (enemiesInArea[i].IsDead())
                {
                    numberOfDeadEnemies++;
                }

            }

            enemiesKilledText.text = numberOfDeadEnemies.ToString() + " / " + enemiesInArea.Length;

            if (numberOfDeadEnemies >= enemiesInArea.Length)
            {
                OpenGate();
            }
        }
    }


    public void OpenGate()
    {
        gateAnim.SetTrigger("Open");
    }
}
