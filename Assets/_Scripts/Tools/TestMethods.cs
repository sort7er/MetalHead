using UnityEngine.Events;
using UnityEngine;

public class TestMethods : MonoBehaviour
{
    public UnityEvent onPressP, onReleaseP, onPressR;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            onPressP.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            onReleaseP.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            onPressR.Invoke();
        }
    }
}
