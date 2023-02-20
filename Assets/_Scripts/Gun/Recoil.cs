using UnityEngine;

public class Recoil : MonoBehaviour
{
    public float recoilSpeedUp, recoilSpeedDown, magnitude;
    public Transform leftHandAttach, rightHandAttach;

    private bool left, recoil;


    private void Update()
    {
        if(left && recoil)
        {
            leftHandAttach.localRotation = Quaternion.Slerp(leftHandAttach.localRotation, Quaternion.Euler(Random.Range(leftHandAttach.localEulerAngles.x, leftHandAttach.localEulerAngles.x - 20), 0, 0), recoilSpeedUp * Time.deltaTime);
        }
        else if (!left && recoil)
        {

        }
    }

    public void StartRecoil()
    {
        CancelInvoke();
        if(GameManager.instance.CheckHand("Pistol") == 1)
        {
            GameManager.instance.leftHand.NewParent(leftHandAttach, leftHandAttach);
            left = true;
        }
        else if(GameManager.instance.CheckHand("Pistol") == 2)
        {
            GameManager.instance.rightHand.NewParent(rightHandAttach, rightHandAttach);
            left = false;
        }
        recoil = true;
    }
    public void EndRecoil()
    {
        GameManager.instance.leftHand.OriginalParent();
    }
}
