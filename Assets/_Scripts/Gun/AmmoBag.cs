using UnityEngine;
using TMPro;

public class AmmoBag : MonoBehaviour
{
    public int numberOfMagsAtStart;
    public Transform magPos;
    public GameObject magazinePrefab;
    public TextMeshProUGUI ammoText;

    private int ammo;
    private GameObject magazineToDrop;
    private Animator ammoPouchAnim;
    private bool handIn;

    private void Start()
    {
        ammoPouchAnim = GetComponent<Animator>();
        AddAmmo(numberOfMagsAtStart);
        Invoke("CheckAmmoStatus", 0.2f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magazine"))
        {
            magazineToDrop = other.gameObject;
        }
        if (other.CompareTag("Interactor"))
        {
            if(magPos.childCount > 0 && GameManager.instance.CheckHand("Magazine") == 0)
            {
                CancelInvoke();
                magPos.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                magPos.GetChild(0).GetComponent<Collider>().enabled = true;
                magPos.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
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
            magazineToDrop = null;
        }
        if (other.CompareTag("Interactor"))
        {
            Invoke("DisableChild", 0.3f);
            Invoke("DisableMagToDrop", 0.3f);
            handIn = false;
            ammoPouchAnim.SetBool("Open", false);
            CheckAmmoStatus();
        }
    }

    private void DisableChild()
    {
        if (magPos.childCount > 0)
        {
            magPos.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            magPos.GetChild(0).GetComponent<Collider>().enabled = false;
            magPos.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
            CheckAmmoStatus();
        }
    }
    public void AddAmmo(int numberOfMags)
    {
        for (int i = 0; i < numberOfMags; i++)
        {
            GameObject magazine = Instantiate(magazinePrefab, magPos.position, magPos.rotation);
            magazine.transform.parent = magPos;
            magazine.GetComponent<MeshRenderer>().enabled = false;
            magazine.GetComponent<Collider>().enabled = false;
            magazine.transform.GetChild(0).gameObject.SetActive(false);
        }
        Invoke("CheckAmmoStatus", 0.1f);
    }
    public void CheckAmmoStatus()
    {
        if(magPos.childCount > 0)
        {
            ammo = 0;

            for (int i = 0; i < magPos.childCount; i++)
            {
                Mag mag = magPos.GetChild(i).GetComponent<Mag>();
                ammo += mag.GetCurrentAmmoFromMag();
            }

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
            magazineToDrop.GetComponent<MeshRenderer>().enabled = false;
            magazineToDrop.GetComponent<Collider>().enabled = false;
            magazineToDrop.transform.GetChild(0).gameObject.SetActive(false);
            magazineToDrop = null;
            CheckAmmoStatus();
        }

    }

}
