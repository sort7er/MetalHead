using TMPro;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public float smoothTimeUp, smoothTimeDown;
    public Transform leftHandAttach, rightHandAttach;

    private bool left, recoil, moveUp;
    private float currentRotation, currentPosition, targetRotation, targetPosition, velocity, velocity2,  lowestAngle, highestAngle, recoilHeight;

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

        if (GameManager.instance.CheckHand("Gun") == 1)
        {
            GameManager.instance.leftHand.NewParent(leftHandAttach, leftHandAttach);
            left = true;
        }
        else if (GameManager.instance.CheckHand("Gun") == 2)
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

    public void UpgradeRecoil(int currentLvl)
    {
        if(currentLvl == 1)
        {
            lowestAngle = 15;
            highestAngle = 20;
            recoilHeight = 0.04f;
        }
        else if (currentLvl == 2)
        {
            lowestAngle = 10;
            highestAngle = 15;
            recoilHeight = 0.03f;
        }
        else if (currentLvl == 3)
        {
            lowestAngle = 5;
            highestAngle = 10;
            recoilHeight = 0.02f;
        }
        else if (currentLvl == 4)
        {
            lowestAngle = 0;
            highestAngle = 5;
            recoilHeight = 0.01f;
        }
        else
        {
            Debug.Log("FullyUpgraded");
        }
    }
}
