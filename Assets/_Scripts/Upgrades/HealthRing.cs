using UnityEngine;
using UnityEngine.UIElements;

public class HealthRing : MonoBehaviour
{

    public float startSmoothTime;

    private Vector3 targetPos;
    private PlayerHealth playerHealth;
    private float smoothTime;
    private bool left, pickedUp, grabbed;

    private void Start()
    {
        smoothTime = startSmoothTime;
        playerHealth = GameManager.instance.XROrigin.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if(grabbed)
        {

            if (left)
            {
                targetPos = GameManager.instance.leftHand.transform.position;
            }
            else
            {
                targetPos = GameManager.instance.rightHand.transform.position;
            }

            smoothTime += 0.2f;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, smoothTime * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) <= 0.05f && !pickedUp)
            {
                pickedUp = true;
                playerHealth.UpgradeHealth();
                Destroy(gameObject);
            }
        }
    }

    public void GrabRing()
    {
        if (!grabbed)
        {
            if (GameManager.instance.CheckGameObject(gameObject) == 1)
            {
                left = true;
            }
            else if (GameManager.instance.CheckGameObject(gameObject) == 2)
            {
                left = false;
            }
            grabbed = true;
        }

    }
}
