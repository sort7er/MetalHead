using TMPro;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public Transform magnetMuzzle;
    public Dial doubleDial, singleDial, thirdDial, fourthDial, fifthDial, sixthDial;
    private int metalsCollected;
    private int singleDigit, doubleDigit, thirdDigit, fourthDigit, fifthDigit, sixthDigit;

    private void Start()
    {
        UpdateMetal(4000);
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
