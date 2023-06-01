using UnityEngine;

public class MagnetSocket : MonoBehaviour
{
    public Transform magnet, attachTransform;
    public UpgradeStation upgradeStation;

    private Animator magnetSocketAnim;
    private Rigidbody rb;
    private ReturnToHolster returnToHolster;
    private bool canDrop, inserted, returnToPlayer;

    private void Start()
    {
        magnetSocketAnim = GetComponent<Animator>();
        returnToHolster = magnet.GetComponent<ReturnToHolster>();
        rb = magnet.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (canDrop && !returnToPlayer)
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
        if(inserted && Vector3.Distance(GameManager.instance.XROrigin.transform.position, magnet.position) > 4 && !returnToPlayer)
        {
            EjectMagnet();
            returnToPlayer = true;
            GameManager.instance.IsUpgrading(false);
        }
        else if(returnToPlayer && returnToHolster.isHolding)
        {
            returnToPlayer = false;
        }
    }

    private void InsertMagnet()
    {
        upgradeStation.Screen();
        inserted = true;
        returnToHolster.IsBeeingUpgraded(true);
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
        returnToHolster.IsBeeingUpgraded(false);

    }
    public void CanDrop(bool state)
    {
        canDrop = state;
        magnetSocketAnim.SetBool("Open", state);
    }
}
