using UnityEngine;

public class InsertWeapon : MonoBehaviour
{
    public UpgradeStation upgradeStation;
    public Transform upgradePosition;

    private ReturnToHolster returnToHolster;
    private Rigidbody weaponsRigidbody;
    private Animator insertWeaponAnim;

    [HideInInspector] public bool inserted;
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
            weaponsRigidbody = other.GetComponent<Rigidbody>();
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gun"))
        {
            weaponInserted = 0;
            returnToHolster = null;
            weaponsRigidbody = null;
        }
    }

    private void Update()
    {
        if(weaponInserted == 1)
        {
            if (!returnToHolster.isHolding && !returnToHolster.isHolstered && !inserted)
            {
                IsInserted(true);
                upgradeStation.WeaponIn(weaponInserted);
                returnToHolster.enabled = false;
                weaponsRigidbody.isKinematic = true;
                weaponsRigidbody.useGravity = false;
                weaponsRigidbody.transform.parent = transform;
                weaponsRigidbody.transform.position = upgradePosition.position;
                weaponsRigidbody.transform.rotation = upgradePosition.rotation;
                InsertWeaponAnim(false);
            }
            if(returnToHolster.isHolding && inserted)
            {
                EjectWeapon();
            }
        }
    }

    public void EjectWeapon()
    {
        if(inserted)
        {
            returnToHolster.enabled = true;
            weaponsRigidbody.transform.parent = null;

            weaponInserted = 0;
            returnToHolster = null;
            weaponsRigidbody = null;

            IsInserted(false);

            if(!upgradeStation.isOn)
            {
                InsertWeaponAnim(false);
            }
        }
    }
    public void PowerOn()
    {
        if (inserted)
        {
            upgradeStation.WeaponIn(weaponInserted);
            InsertWeaponAnim(false);
        }
    }
    public void PowerOff()
    {
        if (inserted)
        {
            InsertWeaponAnim(true);
        }
        else
        {
            InsertWeaponAnim(false);
        }
    }

    public void InsertWeaponAnim(bool state)
    {
        insertWeaponAnim.SetBool("DrawerOut", state);
    }
    public void IsInserted(bool state)
    {
        inserted= state;
    }
}
