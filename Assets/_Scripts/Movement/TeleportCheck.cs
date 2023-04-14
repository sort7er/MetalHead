using UnityEngine;

public class TeleportCheck : MonoBehaviour
{

    private bool cannotTeleport;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            cannotTeleport = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            cannotTeleport = false;
        }
    }

    public bool CannotTeleport()
    {
        return cannotTeleport;
    }
}
