using UnityEngine;

public class Kickable : MonoBehaviour
{
    [HideInInspector] public bool isKicked;
    public void Kicked(bool state)
    {
        isKicked = state;
    }
}
