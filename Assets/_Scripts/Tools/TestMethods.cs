using UnityEngine.Events;
using UnityEngine;

public class TestMethods : MonoBehaviour
{
    public UnityEvent onPressP, onReleaseP, onPressR, onPressO, onPressU, onPressArrowUp, onPressArrowDown, onPressArrowLeft, onPressArrowRight;

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
        if (Input.GetKeyDown(KeyCode.O))
        {
            onPressO.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            onPressU.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            onPressArrowUp.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            onPressArrowDown.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            onPressArrowLeft.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            onPressArrowRight.Invoke();
        }
    }

    public void One()
    {
        Debug.Log("1");
    }
    public void Two()
    {
        Debug.Log("2");
    }
    public void Three()
    {
        Debug.Log("3");
    }
    public void Four()
    {
        Debug.Log("4");
    }

}
