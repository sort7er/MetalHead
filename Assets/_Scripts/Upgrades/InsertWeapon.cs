using UnityEngine;

public class InsertWeapon : MonoBehaviour
{
    public UpgradeStation upgradeStation;
    public Transform upgradePosition;
    public Transform insertParent;

    private ReturnToHolster returnToHolster, tempReturnToHolster;
    private Rigidbody weaponsRigidbody;
    private Animator insertWeaponAnim;

    [HideInInspector] public bool inserted;
    private int weaponInserted;
    private bool returnToPlayer;

    private void Start()
    {
        insertWeaponAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CZ50>() != null)
        {
            weaponInserted = 1;
            returnToHolster = other.GetComponent<ReturnToHolster>();
            weaponsRigidbody = other.GetComponent<Rigidbody>();
        }
        else if (other.GetComponentInParent<Tac14>() != null)
        {
            weaponInserted = 2;
            returnToHolster = other.GetComponentInParent<ReturnToHolster>();
            weaponsRigidbody = returnToHolster.GetComponent<Rigidbody>();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Tac14>() != null || other.GetComponent<CZ50>() != null)
        {
            weaponInserted = 0;
            returnToHolster = null;
            weaponsRigidbody = null;
        }
    }

    private void Update()
    {
        if(weaponInserted != 0)
        {
            if (!returnToHolster.isHolding && !returnToHolster.isHolstered && !inserted)
            {
                IsInserted(true);
                upgradeStation.WeaponIn(weaponInserted);
                returnToHolster.IsBeeingUpgraded(true);
                returnToHolster.enabled = false;
                weaponsRigidbody.isKinematic = true;
                weaponsRigidbody.useGravity = false;
                weaponsRigidbody.transform.parent = insertParent;
                weaponsRigidbody.transform.position = upgradePosition.position;
                weaponsRigidbody.transform.rotation = upgradePosition.rotation;
                InsertWeaponAnim(false);
            }
            if (returnToHolster.isHolding && inserted)
            {
                EjectWeapon();
            }

            if (inserted && Vector3.Distance(GameManager.instance.XROrigin.transform.position, weaponsRigidbody.position) > 4 && !returnToPlayer)
            {
                tempReturnToHolster = returnToHolster;
                EjectWeapon();
                returnToPlayer = true;
            }
            else if (returnToPlayer && tempReturnToHolster.isHolding)
            {
                returnToPlayer = false;
                tempReturnToHolster = null;
            }
        }
        


    }

    public void EjectWeapon()
    {
        if(inserted)
        {
            returnToHolster.enabled = true;
            returnToHolster.IsBeeingUpgraded(false);
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
            EjectWeapon();
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
