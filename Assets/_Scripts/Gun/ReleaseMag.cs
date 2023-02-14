using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ReleaseMag : MonoBehaviour
{
    public string tagToCompare;
    public InputActionAsset releaseMagInputAction;

    private ReturnToHolster returnToHolster;
    private InputAction releaseMag;
    private XRDirectInteractor rHand, lHand;

    private void Start()
    {
        returnToHolster = GetComponent<ReturnToHolster>();
        lHand = GameManager.instance.leftHand.gameObject.GetComponent<XRDirectInteractor>();
        rHand = GameManager.instance.rightHand.gameObject.GetComponent<XRDirectInteractor>();
    }

    private void Update()
    {
        if(returnToHolster.isHolding)
        {
            if (rHand.selectTarget != null && rHand.selectTarget.CompareTag(tagToCompare))
            {
                Debug.Log("right");
            }
            else if (lHand.selectTarget != null && lHand.selectTarget.CompareTag(tagToCompare))
            {
                Debug.Log("left");
            }
        }
    }
}
