using UnityEngine;

public class AmmoBag : MonoBehaviour
{
    public int numberOfMagsAtStart;
    public Transform magPos;
    public GameObject magazinePrefab;

    private GameObject magazineToDrop;
    private Animator ammoPouchAnim;
    private bool handIn;

    private void Start()
    {
        ammoPouchAnim = GetComponent<Animator>();
        AddAmmo(numberOfMagsAtStart);
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
                magPos.GetChild(0).gameObject.SetActive(true);
            }            
            handIn= true;
            ammoPouchAnim.SetBool("Open", true);
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
        }
    }

    private void DisableChild()
    {
        if (magPos.childCount > 0)
        {
            magPos.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void AddAmmo(int numberOfMags)
    {
        for (int i = 0; i < numberOfMags; i++)
        {
            GameObject magazine = Instantiate(magazinePrefab, magPos.position, magPos.rotation);
            magazine.transform.parent = magPos;
            magazine.SetActive(false);
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
        }
    }
    private void DisableMagToDrop()
    {
        if(magazineToDrop != null)
        {
            magazineToDrop.SetActive(false);
            magazineToDrop = null;
        }

    }

}
