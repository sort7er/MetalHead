using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;

    private void Awake()
    {
        instance = this;
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
}
