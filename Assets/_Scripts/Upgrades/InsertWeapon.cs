using UnityEngine;

public class InsertWeapon : MonoBehaviour
{
    public UpgradeStation upgradeStation;
    public Transform upgradePosition;

    private ReturnToHolster returnToHolster;
    private Rigidbody rb;

    public bool inserted, canInsert;

    private void OnTriggerEnter(Collider other)
    {
        if (!inserted && canInsert)
        {
            if (other.GetComponent<CZ50>() != null && other.CompareTag("Gun"))
            {
                upgradeStation.WeaponIn(0);
                returnToHolster = other.GetComponent<ReturnToHolster>();
                rb = other.GetComponent<Rigidbody>();
                other.transform.position = upgradePosition.position;
                other.transform.rotation = upgradePosition.rotation;
                Insert();
            }
        }
    }

    private void Insert()
    {
        inserted = true; 
        rb.isKinematic = true;
        rb.useGravity = false;
        returnToHolster.enabled = false;
    }

    public void Eject()
    {
        if(rb != null)
        {
            returnToHolster.enabled = true;
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        rb = null;
        returnToHolster = null;
        inserted = false;
    }

    public void CanInsert(bool state)
    {
        canInsert = state;
    }
}
