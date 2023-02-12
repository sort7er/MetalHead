using UnityEngine.Events;
using UnityEngine;

public class TestMethods : MonoBehaviour
{
    public UnityEvent onPressP;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            onPressP.Invoke();
        }
    }
}
