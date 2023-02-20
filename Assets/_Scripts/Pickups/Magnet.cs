using UnityEngine;

public class Magnet : MonoBehaviour
{
    public Transform magnetMuzzle;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            other.GetComponent<Pickup>().PickUp(magnetMuzzle);
        }
    }
}
