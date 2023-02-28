using TMPro;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public Transform magnetMuzzle;
    public TextMeshProUGUI metalsText;

    private int metalsCollected = 1000;

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
        metalsText.text = metalsCollected.ToString("00000");
    }

    public int GetMetalsCollected()
    {
        return metalsCollected;
    }
    public void SetMetalsCollected(int newNumber)
    {
        metalsCollected = newNumber;
    }
}
