using TMPro;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public float smoothTimeUp, smoothTimeDown, lowestAngle, highestAngle, recoilHeight;
    public Transform leftHandAttach, rightHandAttach;

    private bool left, recoil, moveUp;
    private float currentRotation, currentPosition, targetRotation, targetPosition, velocity, velocity2;


    private void Update()
    {
        if (left && recoil)
        {
            leftHandAttach.localRotation = Quaternion.Euler(currentRotation, 0, 0);
            leftHandAttach.localPosition = new Vector3(0, currentPosition, 0);

            if (moveUp)
            {
                currentRotation = Mathf.SmoothDampAngle(currentRotation, targetRotation, ref velocity, Time.deltaTime * smoothTimeUp);
                currentPosition = Mathf.SmoothDamp(currentPosition, targetPosition, ref velocity2, Time.deltaTime * smoothTimeUp);
                if (currentRotation <= targetRotation + 0.1f)
                {
                    moveUp = false;
                    targetPosition = 0;
                    targetRotation = 0;
                }
            }
            else
            {
                currentRotation = Mathf.SmoothDampAngle(currentRotation, targetRotation, ref velocity, Time.deltaTime * smoothTimeDown);
                currentPosition = Mathf.SmoothDamp(currentPosition, targetPosition, ref velocity2, Time.deltaTime * smoothTimeDown);
                if (leftHandAttach.localRotation.x >= 0)
                {
                    leftHandAttach.localRotation = Quaternion.Euler(0, 0, 0);
                    EndRecoil();
                }
            }
        }
        else if (!left && recoil)
        {
            rightHandAttach.localRotation = Quaternion.Euler(currentRotation, 0, 0);
            rightHandAttach.localPosition = new Vector3(0, currentPosition, 0);

            if (moveUp)
            {
                currentRotation = Mathf.SmoothDampAngle(currentRotation, targetRotation, ref velocity, Time.deltaTime * smoothTimeUp);
                currentPosition = Mathf.SmoothDamp(currentPosition, targetPosition, ref velocity2, Time.deltaTime * smoothTimeUp);
                if (currentRotation <= targetRotation + 0.1f)
                {
                    moveUp = false;
                    targetPosition = 0;
                    targetRotation = 0;
                }
            }
            else
            {
                currentRotation = Mathf.SmoothDampAngle(currentRotation, targetRotation, ref velocity, Time.deltaTime * smoothTimeDown);
                currentPosition = Mathf.SmoothDamp(currentPosition, targetPosition, ref velocity2, Time.deltaTime * smoothTimeDown);
                if (rightHandAttach.localRotation.x >= 0)
                {
                    rightHandAttach.localRotation = Quaternion.Euler(0, 0, 0);
                    EndRecoil();
                }
            }
        }
    }

    public void StartRecoil()
    {

        if (GameManager.instance.CheckHand("Pistol") == 1)
        {
            GameManager.instance.leftHand.NewParent(leftHandAttach, leftHandAttach);
            left = true;
        }
        else if (GameManager.instance.CheckHand("Pistol") == 2)
        {
            GameManager.instance.rightHand.NewParent(rightHandAttach, rightHandAttach);
            left = false;
        }
        targetRotation = -Random.Range(lowestAngle, highestAngle);
        targetPosition = recoilHeight;
        moveUp = true;
        recoil = true;
    }
    public void EndRecoil()
    {
        recoil = false;
        GameManager.instance.leftHand.OriginalParent();
    }
}
