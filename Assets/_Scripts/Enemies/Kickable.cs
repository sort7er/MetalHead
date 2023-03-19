using UnityEngine;

public class Kickable : MonoBehaviour
{
    [HideInInspector] public bool isBeeingKicked;
    public void IsBeeingKicked(bool state)
    {
        isBeeingKicked = state;
    }
}
