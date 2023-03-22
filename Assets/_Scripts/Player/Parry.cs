using UnityEngine;

public class Parry : MonoBehaviour
{
    [HideInInspector] public bool isParrying;


    public void StartParry()
    {
        isParrying = true;
        Invoke(nameof(DoneParrying), 0.05f);
    }
    public void DoneParrying()
    {
        isParrying = false;
    }
}
