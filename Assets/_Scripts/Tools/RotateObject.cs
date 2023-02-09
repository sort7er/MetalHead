using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeedX, rotationSpeedY, rotationSpeedZ;

    private void Update()
    {
        transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
    }
}
