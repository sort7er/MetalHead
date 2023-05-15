using UnityEngine;
using TMPro;

public class AmmoBag : MonoBehaviour
{
    public int bulletsToStartWith;
    public Transform magPos;
    public GameObject magazinePrefab;
    public TextMeshProUGUI ammoText;

    [HideInInspector] public bool tutorialCheck;

    private int ammo;
    private GameObject magazineToDrop;
    private Animator ammoPouchAnim;
    private bool handIn;
    private int rest, numberOfMags;

    private void Start()
    {
        ammoPouchAnim = GetComponent<Animator>();
        ammo = bulletsToStartWith;
        UpdateAmmo();
        if(FindObjectOfType<TutorialManager>() != null)
        {
            tutorialCheck = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magazine"))
        {
            CancelInvoke();
            magazineToDrop = other.gameObject;
            handIn = true;
            ammoPouchAnim.SetBool("Open", true);
            CheckAmmoStatus();
        }
        if (other.CompareTag("Interactor") && GameManager.instance.CheckHand("Magazine") == 0)
        {
            if(magPos.childCount > 0)
            {
                foreach (Transform m in magPos)
                {
                    m.gameObject.SetActive(false);
                }
                CancelInvoke();
                magPos.GetChild(0).gameObject.SetActive(true);
                magPos.GetChild(0).GetComponent<SphereCollider>().enabled = true;
            }            
            handIn= true;
            ammoPouchAnim.SetBool("Open", true);
            CheckAmmoStatus();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Magazine"))
        {
            if (tutorialCheck)
            {
                if(other.GetComponent<Mag>().GetCurrentAmmoFromMag() > 0)
                {
                    tutorialCheck = false;
                }
            }
            magazineToDrop = null;
            handIn = false;
            ammoPouchAnim.SetBool("Open", false);
            CheckAmmoStatus();
        }
        if (other.CompareTag("Interactor") && GameManager.instance.CheckHand("Magazine") == 0)
        {
            Invoke(nameof(DisableMagToDrop), 0.2f);
            handIn = false;
            if(magPos.childCount > 0)
            {
                magPos.GetChild(0).GetComponent<SphereCollider>().enabled = false;
            }
            ammoPouchAnim.SetBool("Open", false);
            CheckAmmoStatus();
        }
    }
    public void AddAmmo(int numberOfBullets)
    {
        ammo += numberOfBullets;
        UpdateAmmo();
    }
    public void UpdateAmmo()
    {
        rest = ammo % UpgradeManager.instance.magSize;
        numberOfMags = (ammo - rest) / UpgradeManager.instance.magSize;
        ammoText.text = ammo.ToString();

        if (magPos.childCount > 0)
        {
            foreach (Transform m in magPos)
            {
                Destroy(m.gameObject);
            }
        }

        for (int i = 0; i < numberOfMags; i++)
        {
            GameObject magazine = Instantiate(magazinePrefab, magPos.position, magPos.rotation);
            magazine.transform.parent = magPos;
            magazine.SetActive(false);
        }

        if(rest > 0)
        {
            GameObject magazineWithLessBullets = Instantiate(magazinePrefab, magPos.position, magPos.rotation);
            magazineWithLessBullets.transform.parent = magPos;
            magazineWithLessBullets.GetComponent<Mag>().SetMag(rest);
            magazineWithLessBullets.SetActive(false);
        }
    }
    public void CheckAmmoStatus()
    {
        if(magPos.childCount > 0)
        {
            int newAmmo = 0;
            Mag[] mags;

            mags = magPos.gameObject.GetComponentsInChildren<Mag>(true);

            foreach(Mag m in mags)
            {
                newAmmo += m.GetCurrentAmmoFromMag();
            }

            ammo = newAmmo;
            ammoText.text = ammo.ToString();
        }
        else
        {
            ammo = 0;
            ammoText.text = ammo.ToString();
        }
    }
    public void ReleasingMag()
    {
        CancelInvoke();
        if (handIn && magazineToDrop != null)
        {

            magazineToDrop.GetComponent<Mag>().EnableGravity(false);
            magazineToDrop.transform.parent = magPos;
            magazineToDrop.transform.position = magPos.position;
            magazineToDrop.transform.rotation = magPos.rotation;
            CheckAmmoStatus();
        }
    }
    private void DisableMagToDrop()
    {
        if(magazineToDrop != null)
        {
            magazineToDrop.SetActive(false);
            magazineToDrop = null;
            if (magPos.childCount > 0)
            {
                magPos.GetChild(0).gameObject.SetActive(false);
            }
            CheckAmmoStatus();
            UpdateAmmo();
        }

    }
}
