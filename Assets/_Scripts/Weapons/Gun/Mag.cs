using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Mag : MonoBehaviour
{
    public Transform leftAttach, rightAttach;
    public XRGrabInteractable magFull;
    public Transform magPart;
    public Transform[] magPartPos;
    public GameObject[] bullets;
    public Collider trigger;
    public MeshRenderer[] bulbs;
    public Material gotAmmo, noAmmo;

    private int magSize;
    private int currentAmmo;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        if (UpgradeManager.instance != null)
        {
            magSize = UpgradeManager.instance.magSize;
            currentAmmo = magSize;
        }
        else
        {
            magSize = 10;
            currentAmmo = magSize;
        }

    }

    private void Start()
    {
        
        meshRenderer.enabled = true;
        rb = GetComponent<Rigidbody>();
        EnableGravity(false);
        for (int i = 0; i < bulbs.Length; i++)
        {
            bulbs[i].material = gotAmmo;
        }
    }

    public void GrabMag()
    {
        CancelInvoke();
        trigger.enabled = false;
        if (GameManager.instance.CheckHand("Magazine") == 1)
        {
            magFull.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabMag(true);
        }
        if (GameManager.instance.CheckHand("Magazine") == 2)
        {
            magFull.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabMag(true);
        }
    }
    public void ReleaseMag()
    {
        trigger.enabled = true;
        GameManager.instance.leftHand.GrabMag(false);
        GameManager.instance.rightHand.GrabMag(false);
        transform.parent = ParentManager.instance.mags;
        EnableGravity(true);
        CheckForDestroyMag();
        GameManager.instance.ammoBag.ReleasingMag();
    }
    public void CheckForDestroyMag()
    {
        if (currentAmmo <= 0)
        {
            trigger.enabled = false;
            Invoke("DestroyMag", 15);
        }
    }
    private void DestroyMag()
    {
        Destroy(gameObject);
    }

    public void Fire()
    {
        currentAmmo--;
        if (currentAmmo == 0)
        {
            for (int i = 0; i < bulbs.Length; i++)
            {
                bulbs[i].material = noAmmo;
            }
            bullets[0].SetActive(false);
        }
        else if (currentAmmo == 1)
        {
            bullets[1].SetActive(false);
            magPart.position = magPartPos[3].position;
        }
        else if (currentAmmo == 2)
        {
            bullets[2].SetActive(false);
            magPart.position = magPartPos[2].position;
        }
        else if (currentAmmo == 3)
        {
            bullets[3].SetActive(false);
            magPart.position = magPartPos[1].position;
        }
        else
        {
            magPart.position = magPartPos[0].position;
        }

    }
    public void UpgradeMags()
    {
        currentAmmo = UpgradeManager.instance.magSize;
    }
    public void SetMag(int ammo)
    {
        currentAmmo = ammo;
        if (currentAmmo == 0)
        {
            bullets[0].SetActive(false);
            bullets[1].SetActive(false);
            bullets[2].SetActive(false);
            bullets[3].SetActive(false);
        }
        else if (currentAmmo == 1)
        {
            bullets[1].SetActive(false);
            bullets[2].SetActive(false);
            bullets[3].SetActive(false);
            magPart.position = magPartPos[3].position;
        }
        else if (currentAmmo == 2)
        {
            bullets[2].SetActive(false);
            bullets[3].SetActive(false);
            magPart.position = magPartPos[2].position;
        }
        else if (currentAmmo == 3)
        {
            bullets[3].SetActive(false);
            magPart.position = magPartPos[1].position;
        }
        else
        {
            magPart.position = magPartPos[0].position;
        }
    }
    public int GetCurrentAmmoFromMag()
    {
        return currentAmmo;
    }
    public void EnableGravity(bool state)
    {
        if(rb != null)
        {
            rb.useGravity= state;
            rb.isKinematic= !state;
        }
    }
}
