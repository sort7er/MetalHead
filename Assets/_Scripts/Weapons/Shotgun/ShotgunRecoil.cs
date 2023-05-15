using UnityEngine;

public class ShotgunRecoil : MonoBehaviour
{
    public float defaultSmoothTimeUp, defaultSmoothTimeDown;
    public Transform leftHandAttach, rightHandAttach;
    public Transform leftWeaponAttach, rightWeaponAttach;
    
    private Vector3 leftStartPos, rightStartPos;
    private bool left, recoil, moveUp;
    private float currentRotation, currentPosition, targetRotation, targetPosition, velocity, velocity2, lowestAngle, highestAngle, recoilHeight;
    private float smoothTimeUp, smoothTimeDown;

    private void Start()
    {
        leftStartPos = leftHandAttach.localPosition;
        rightStartPos = rightHandAttach.localPosition;
        SetRecoil(true);
    }


    private void Update()
    {
        if (left && recoil)
        {
            leftHandAttach.localRotation = Quaternion.Euler(currentRotation, 0, 0);
            leftHandAttach.localPosition = new Vector3(leftStartPos.x, currentPosition, leftStartPos.z);

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
                    leftHandAttach.localPosition = leftStartPos;
                    leftHandAttach.localRotation = Quaternion.Euler(0, 0, 0);
                    EndRecoil();
                }
            }
        }
        else if (!left && recoil)
        {
            rightHandAttach.localRotation = Quaternion.Euler(currentRotation, 0, 0);
            rightHandAttach.localPosition = new Vector3(rightStartPos.x, currentPosition, rightStartPos.z);

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
                    rightHandAttach.localPosition = rightStartPos;
                    rightHandAttach.localRotation = Quaternion.Euler(0, 0, 0);
                    EndRecoil();
                }
            }
        }
    }

    public void StartRecoil()
    {

        if (GameManager.instance.CheckGameObject(gameObject) == 1)
        {
            left = true;
        }
        else if (GameManager.instance.CheckGameObject(gameObject) == 2)
        {
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
    }

    public void SetRecoil(bool oneHand)
    {
        if (oneHand)
        {
            lowestAngle = 40;
            highestAngle = 55;
            recoilHeight = 0.1f;
            smoothTimeUp = defaultSmoothTimeUp;
            smoothTimeDown = defaultSmoothTimeDown;
        }
        else
        {
            lowestAngle = 10;
            highestAngle = 15;
            recoilHeight = 0.03f;
            smoothTimeUp = 1;
            smoothTimeDown = 10;
        }
    }
}
