using UnityEngine;

public class InsertWeapon : MonoBehaviour
{
    public UpgradeStation upgradeStation;
    public Transform upgradePosition;

    private ReturnToHolster returnToHolster;

    public bool inserted;

    private void OnTriggerEnter(Collider other)
    {
        if (!inserted)
        {
            if (other.GetComponent<CZ50>() != null && other.CompareTag("Gun"))
            {
                upgradeStation.WeaponIn(0);
                Insert();
            }
        }
    }

    private void Insert()
    {
        inserted = true; 
    }

}
