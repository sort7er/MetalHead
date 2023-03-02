using TMPro;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public Transform magnetMuzzle;
    public TextMeshProUGUI metalsText;

    private int metalsCollected;

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
        if (metalsCollected == 0)
        {
            metalsText.text = "0";
        }
        else
        {
            metalsText.text = metalsCollected.ToString("#,#");
        }
    }

    public int GetMetalsCollected()
    {
        return metalsCollected;
    }
    public void SetMetalsCollected(int newNumber)
    {
        metalsCollected = newNumber;
        if(metalsCollected == 0)
        {
            metalsText.text = "0";
        }
        else
        {
            metalsText.text = metalsCollected.ToString("#,#");
        }
    }
}
