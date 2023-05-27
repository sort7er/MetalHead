using UnityEngine;

public class Guide : MonoBehaviour
{
    public float Xamount, Yamount;

    private Vector3 targetPosition, offset;
    private Transform cam;


    void Update()
    {
        cam = GameManager.instance.cam.transform;

        transform.rotation = cam.rotation;

        targetPosition = cam.position + cam.forward + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 20);
    }

    public void SetOffset(int positionLikeAPhone)
    {
        if(positionLikeAPhone == 1)
        {
            offset = -cam.right * Xamount + cam.up * Yamount;
        }
        else if(positionLikeAPhone == 2)
        {
            offset = cam.up * Yamount;
        }
        else if (positionLikeAPhone == 3)
        {
            offset = cam.right * Xamount + cam.up * Yamount;
        }
        else if (positionLikeAPhone == 4)
        {
            offset = -cam.right * Xamount;
        }
        else if (positionLikeAPhone == 5)
        {
            offset = Vector3.zero;
        }
        else if (positionLikeAPhone == 6)
        {
            offset = cam.right * Xamount;
        }
        else if (positionLikeAPhone == 7)
        {
            offset = -cam.right * Xamount - cam.up * Yamount;
        }
        else if (positionLikeAPhone == 8)
        {
            offset = -cam.up * Yamount;
        }
        else
        {
            offset = cam.right * Xamount - cam.up * Yamount;
        }


    }
}
