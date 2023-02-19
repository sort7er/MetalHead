using UnityEngine;

public class AmmoBag : MonoBehaviour
{
    public int maxAmmo;

    public GameObject magazinePrefab;
    public Transform spawn;

    public GameObject magazine;
    public int currentTotalAmmo, rest, numberOfFullMags, magSize;
    private bool grabbed, handIn;

    private void Start()
    {
        currentTotalAmmo = maxAmmo;  
        UpdateAmmo();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactor"))
        {
            InstatntiateMagazine();
            handIn= true;
        }
        if (other.CompareTag("Magazine"))
        {
            magazine = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactor"))
        {
            DestroyMagazine();
            handIn = false;
        }
        if (other.CompareTag("Magazine"))
        {
            magazine = null;
        }
    }



    public void InstatntiateMagazine()
    {
        if (!grabbed)
        {
            currentTotalAmmo = -magSize;
            magazine = Instantiate(magazinePrefab, spawn.position, spawn.rotation);
            magazine.transform.parent = transform;
            magazine.GetComponent<Rigidbody>().isKinematic = true;
            magazine.GetComponent<Rigidbody>().useGravity = false;
        }
    }
    public void GrabbedMag(bool state)
    { 
        grabbed = state;
        if(!grabbed && handIn)
        {
            DestroyMagazine();
            InstatntiateMagazine();
        }
    }
    public void DestroyMagazine()
    {
        if (!grabbed)
        {
            Destroy(magazine);
        }
    }
    private void UpdateAmmo()
    {
        magSize = UpgradeManager.instance.GetMagSize();
        if (magSize != 0)
        {
            rest = currentTotalAmmo % magSize;
            numberOfFullMags = (currentTotalAmmo - rest) / magSize;
        }
    }
}
