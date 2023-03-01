using UnityEngine;

public class InsertWeapon : MonoBehaviour
{
    public UpgradeStation upgradeStation;
    public Transform upgradePosition;

    private ReturnToHolster returnToHolster;
    private Animator insertWeaponAnim;

    public bool inserted;
    private int weaponInserted;

    private void Start()
    {
        insertWeaponAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CZ50>() != null && other.CompareTag("Gun"))
        {
            weaponInserted = 1;
            returnToHolster = other.GetComponent<ReturnToHolster>();
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gun"))
        {
            weaponInserted = 0;
            returnToHolster = null;

        }
    }

    private void Update()
    {
        if(weaponInserted == 1 && !inserted)
        {
            if (!returnToHolster.isHolding && !returnToHolster.isHolstered) 
            {
                inserted = true;
                upgradeStation.WeaponIn(weaponInserted);
                Debug.Log("inserted");
            }
        }
    }

    public void InsertWeaponAnim(bool state)
    {
        insertWeaponAnim.SetBool("Insert", state);
    }
}
