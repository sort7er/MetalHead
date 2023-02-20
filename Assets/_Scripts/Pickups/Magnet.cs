using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Magnet : MonoBehaviour
{
    public Transform magnetMuzzle;
    public TextMeshProUGUI metalsText;

    private int metalsCollected;

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
}
