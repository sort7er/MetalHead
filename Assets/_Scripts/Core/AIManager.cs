using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;
    public bool playerIsBeeingAttacked;

    private int currentPos;

    private void Awake()
    {
        instance = this;
    }

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
}
