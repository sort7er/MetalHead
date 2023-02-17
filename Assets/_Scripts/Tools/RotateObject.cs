using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeedX, rotationSpeedY, rotationSpeedZ;

    public bool randomizeRotation;

    private void Start()
    {
        if (randomizeRotation)
        {
            rotationSpeedX = Random.Range(-50, 50);
            rotationSpeedY = Random.Range(-50, 50);
            rotationSpeedZ = Random.Range(-50, 50);
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
    }
}
