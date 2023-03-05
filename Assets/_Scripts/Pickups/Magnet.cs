using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Magnet : MonoBehaviour
{

    public Transform magnetMuzzle, leftAttach, rightAttach;
    public Dial doubleDial, singleDial, thirdDial, fourthDial, fifthDial, sixthDial;

    private XRGrabInteractable xrGrabInteractable;
    private Animator magnetAnim;
    private int metalsCollected;
    private int singleDigit, doubleDigit, thirdDigit, fourthDigit, fifthDigit, sixthDigit;

    private void Start()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        UpdateMetal(4000);
        magnetAnim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            other.GetComponent<Pickup>().PickUp(magnetMuzzle);
        }
    }

    public void UpdateMetal(int value)
    {
        metalsCollected += value;
        UpdateDial();
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
        magnetAnim.SetBool("Out", true);
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
        magnetAnim.SetBool("Out", false);
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
