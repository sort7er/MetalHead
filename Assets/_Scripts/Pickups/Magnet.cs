using TMPro;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public Transform magnetMuzzle;
    public TextMeshProUGUI metalsText;

    private int metalsCollected;

    private void Start()
    {
        UpdateMetal(1000);
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
        metalsText.text = metalsCollected.ToString("#,#");
    }

    public int GetMetalsCollected()
    {
        return metalsCollected;
    }
    public void SetMetalsCollected(int newNumber)
    {
        metalsCollected = newNumber;
        metalsText.text = metalsCollected.ToString("#,#");
    }
}
