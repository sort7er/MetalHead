using UnityEngine;
using UnityEngine.UIElements;

public class Dial : MonoBehaviour
{
    public float turnSmoothTime;

    private float targetRotation, currentRotation;

    private void Update()
    {
        currentRotation = Mathf.LerpAngle(currentRotation, targetRotation, turnSmoothTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation, 0, 0);
    }

    public void SetDial(int number)
    {
        targetRotation = 360 - number * 36;
    }
}
