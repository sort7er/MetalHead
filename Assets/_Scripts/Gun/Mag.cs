using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Mag : MonoBehaviour
{
    public int MagSize;
    public Transform leftAttach, rightAttach;
    public XRGrabInteractable magFull;
    public Transform magPart;
    public Transform[] magPartPos;
    public GameObject[] bullets;

    private int currentAmmo;
    private Rigidbody rb;

    private void Start()
    {
        currentAmmo = MagSize;
        rb = GetComponent<Rigidbody>();
    }

    public void GrabMag()
    {
        if (GameManager.instance.CheckHand("Magazine") == 1)
        {
            magFull.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabMag(true);
        }
        if (GameManager.instance.CheckHand("Magazine") == 2)
        {
            magFull.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabMag(true);
        }
    }
    public void ReleaseMag()
    {
        GameManager.instance.leftHand.GrabMag(false);
        GameManager.instance.rightHand.GrabMag(false);
    }

    public void Fire()
    {
        currentAmmo--;
        if (currentAmmo == 0)
        {
            bullets[0].SetActive(false);
        }
        else if (currentAmmo == 1)
        {
            bullets[1].SetActive(false);
            magPart.position = magPartPos[3].position;
        }
        else if (currentAmmo == 2)
        {
            bullets[2].SetActive(false);
            magPart.position = magPartPos[2].position;
        }
        else if (currentAmmo == 3)
        {
            bullets[3].SetActive(false);
            magPart.position = magPartPos[1].position;
        }
        else
        {
            magPart.position = magPartPos[0].position;
        }

    }
    public int GetCurrentAmmoFromMag()
    {
        return currentAmmo;
    }
    public void EnableGravity(bool state)
    {
        rb.useGravity= state;
        rb.isKinematic= !state;
    }
}
