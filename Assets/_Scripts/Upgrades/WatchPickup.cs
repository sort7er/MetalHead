using UnityEngine;

public class WatchPickup : MonoBehaviour
{

    public float startSmoothTime;
    public RequirementCheck requirementCheck;
    public TutorialManager tutorialManager;

    private Vector3 targetPos;
    private float smoothTime;
    private bool left, pickedUp, grabbed;

    private void Start()
    {
        smoothTime = startSmoothTime;
    }

    private void Update()
    {
        if (grabbed)
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
                EffectManager.instance.PickUpRingEffect(tutorialManager.watch.transform.position);
                requirementCheck.GrabbedWatch();
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