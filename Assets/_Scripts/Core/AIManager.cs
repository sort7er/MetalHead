using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;
/*    [HideInInspector] */public RunningEnemy[] runningEnemiesInScene;

    private void Awake()
    {
        instance = this;
        ActualUpdate();
    }
    [HideInInspector] public bool playerIsBeeingAttacked;

    private int currentPos;

    public bool CheckForAttack()
    {
        if (!playerIsBeeingAttacked)
        {
            playerIsBeeingAttacked = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void DoneAttacking()
    {
        playerIsBeeingAttacked = false;
    }
    public void UpdateArray()
    {
        Invoke(nameof(ActualUpdate), 0.1f);
    }
    private void ActualUpdate()
    {
        runningEnemiesInScene = FindObjectsOfType<RunningEnemy>();
    }
}
