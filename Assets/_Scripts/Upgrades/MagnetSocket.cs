using UnityEngine;

public class MagnetSocket : MonoBehaviour
{
    public Transform magnet, attachTransform;
    public UpgradeStation upgradeStation;

    private Rigidbody rb;
    private ReturnToHolster returnToHolster;
    private bool canDrop, inserted;

    private void Start()
    {
        returnToHolster = magnet.GetComponent<ReturnToHolster>();
        rb = magnet.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (canDrop)
        {
            if (!returnToHolster.isHolding && !returnToHolster.isHolstered && !inserted)
            {
                InsertMagnet();
            }
            if (returnToHolster.isHolding && inserted)
            {
                EjectMagnet();
            }
        }
    }

    private void InsertMagnet()
    {
        upgradeStation.Screen();
        inserted = true;
        returnToHolster.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        magnet.position = attachTransform.position;
        magnet.rotation = attachTransform.rotation;
    }
    private void EjectMagnet()
    {
        upgradeStation.Screen();
        inserted = false;
        returnToHolster.enabled = true;
    }
    public void CanDrop(bool state)
    {
        canDrop = state;
    }
}