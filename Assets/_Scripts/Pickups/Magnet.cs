using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Magnet : MonoBehaviour
{

    public Animator rodAnim;
    public ParticleSystem magneticEffect;
    public Transform magnetMuzzle, leftAttach, rightAttach;
    public Dial doubleDial, singleDial, thirdDial, fourthDial, fifthDial, sixthDial;

    private MagnetAnimation magnetAnimation;
    private MagnetSounds magnetSounds;
    private XRGrabInteractable xrGrabInteractable;

    private int metalsCollected;
    private int singleDigit, doubleDigit, thirdDigit, fourthDigit, fifthDigit, sixthDigit;

    private void Start()
    {
        magnetAnimation = GetComponent<MagnetAnimation>();
        magnetSounds = GetComponent<MagnetSounds>();
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        UpdateMetal(6200);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            CancelInvoke();
            other.GetComponent<Pickup>().PickUp(magnetMuzzle);
            rodAnim.SetBool("PickingUp", true);
            magneticEffect.Play();
            magnetSounds.MagnetOn();
        }
    }

    private void DonePickingUp()
    {
        rodAnim.SetBool("PickingUp", false);
        magneticEffect.Stop();
        magnetSounds.MagnetOff();
    }

    public void UpdateMetal(int value)
    {
        metalsCollected += value;
        UpdateDial();
        Invoke("DonePickingUp", 0.5f);
    }

    public int GetMetalsCollected()
    {
        return metalsCollected;
    }
    public void SetMetalsCollected(int newNumber)
    {
        metalsCollected = newNumber;
        UpdateDial();
    }
    public void GrabMagnet()
    {
        magnetAnimation.GrabMagnetRelay();
        if (GameManager.instance.CheckHand("Magnet") == 1)
        {
            xrGrabInteractable.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabHandle(true);
        }
        if (GameManager.instance.CheckHand("Magnet") == 2)
        {
            xrGrabInteractable.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabHandle(true);
        }
    }
    public void ReleaseMagnet()
    {
        magnetAnimation.ReleaseMagnetAnim();
        magnetSounds.MagnetActivate(1);
        GameManager.instance.leftHand.GrabHandle(false);
        GameManager.instance.rightHand.GrabHandle(false);
    }
    private void UpdateDial()
    {
        singleDigit = metalsCollected % 10;
        doubleDigit = (metalsCollected / 10) % 10;
        thirdDigit = (metalsCollected / 100) % 10;
        fourthDigit = (metalsCollected / 1000) % 10;
        fifthDigit = (metalsCollected / 10000) % 10;
        sixthDigit = (metalsCollected / 100000) % 10;

        singleDial.SetDial(singleDigit);
        doubleDial.SetDial(doubleDigit);
        thirdDial.SetDial(thirdDigit);
        fourthDial.SetDial(fourthDigit);
        fifthDial.SetDial(fifthDigit);
        sixthDial.SetDial(sixthDigit);
    }
}
