using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AmmoBagShotgun : MonoBehaviour
{
    public int slugsToStartWith;
    public Transform slugPos;
    public GameObject slugPrefab;
    public TextMeshProUGUI ammoText;
    public Tac14 tac14;
    private int ammo, ammoToAdd;

    private GameObject slugToDrop, currentSlug;
    private Animator ammoPouchAnim;
    private bool handIn;
    private bool isAllreadyCounting;

    private void Start()
    {
        ammoPouchAnim = GetComponent<Animator>();
        ammo = slugsToStartWith;
        UpdateAmmo();
        UpdateAmmoText();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slug"))
        {
            CancelInvoke();
            slugToDrop = other.gameObject;
            handIn = true;
            ammoPouchAnim.SetBool("Open", true);
            UpdateAmmoText();
        }
        if (other.CompareTag("Interactor") && GameManager.instance.CheckHand("Slug") == 0)
        {
            if (slugPos.childCount > 0)
            {
                foreach (Transform m in slugPos)
                {
                    m.gameObject.SetActive(false);
                }
                CancelInvoke();
                slugPos.GetChild(0).gameObject.SetActive(true);
                currentSlug = slugPos.GetChild(0).gameObject;

                //if(ammo - tac14.insertAmmo < 0)
                //{
                //    ammo = 0;
                //}
                //else
                //{
                //    ammo -= tac14.insertAmmo;
                //}
                ammo--;

            }
            handIn = true;
            ammoPouchAnim.SetBool("Open", true);
            UpdateAmmoText();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slug"))
        {
            slugToDrop = null;
            handIn = false;
            ammoPouchAnim.SetBool("Open", false);
            UpdateAmmoText();
        }
        if (other.CompareTag("Interactor") && GameManager.instance.CheckHand("Slug") == 0)
        {
            Invoke(nameof(DisableMagToDrop), 0.2f);
            currentSlug = null;
            handIn = false;
            ammoPouchAnim.SetBool("Open", false);
            UpdateAmmoText();
        }
    }
    public void UpdateAmmo()
    {
        if (slugPos.childCount > 0)
        {
            foreach (Transform m in slugPos)
            {
                Destroy(m.gameObject);
            }
        }

        for (int i = 0; i < ammo; i++)
        {
            GameObject magazine = Instantiate(slugPrefab, slugPos.position, slugPos.rotation);
            magazine.transform.parent = slugPos;
            magazine.SetActive(false);
        }
    }
    public void AddAmmo(int numberOfSlugs)
    {
        ammo += numberOfSlugs;
        UpdateAmmo();
    }
    public void UpdateAmmoText()
    {
        int newAmmo = 0;
        Slug[] slugs;

        slugs = slugPos.gameObject.GetComponentsInChildren<Slug>(true);

        foreach (Slug s in slugs)
        {
            newAmmo++;
        }

        ammo = newAmmo;

        ammoText.text = ammo.ToString();
        
    }
    public void GrabbingSlug(GameObject slugInQuestion)
    {
        if(slugInQuestion == currentSlug)
        {
            Slug[] slugs;

            slugs = slugPos.gameObject.GetComponentsInChildren<Slug>(true);
            ammoToAdd = 1;

            if (tac14.insertAmmo - 1 < slugs.Length)
            {
                if(tac14.currentAmmo + tac14.insertAmmo <= tac14.magSize)
                {
                    for (int i = 0; i < tac14.insertAmmo - 1; i++)
                    {
                        Debug.Log("1");
                        ammoToAdd++;
                        Destroy(slugPos.GetChild(i).gameObject);
                    }
                }
                else
                {
                    for (int i = 0; i < tac14.magSize - tac14.currentAmmo - 1; i++)
                    {
                        Debug.Log("2");
                        ammoToAdd++;
                        Destroy(slugPos.GetChild(i).gameObject);
                    }
                }
            }
            else
            {
                if (slugs.Length > tac14.magSize - tac14.currentAmmo - 1)
                {
                    for (int i = 0; i < tac14.magSize - tac14.currentAmmo - 1; i++)
                    {
                        Debug.Log("3");
                        ammoToAdd++;
                        Destroy(slugPos.GetChild(i).gameObject);
                    }
                }
                else
                {
                    foreach (Transform m in slugPos)
                    {
                        Debug.Log("4");
                        ammoToAdd++;
                        Destroy(m.gameObject);
                    }
                }
            }
            UpdateAmmoText(); 
        }
    }
    public void ReleasingSlug()
    {
        CancelInvoke();
        if (handIn && slugToDrop != null)
        {

            AddAmmo(ammoToAdd - 1);

            slugToDrop.GetComponent<Slug>().EnableGravity(false);
            slugToDrop.transform.parent = slugPos;
            slugToDrop.transform.position = slugPos.position;
            slugToDrop.transform.rotation = slugPos.rotation;

            //if (tac14.currentAmmo + tac14.insertAmmo > tac14.magSize)
            //{
            //    ammo += (tac14.currentAmmo + tac14.insertAmmo) - tac14.magSize;
            //}
            //else
            //{
            //    ammo += tac14.insertAmmo;
            //}




            ammo++;
            UpdateAmmoText();
        }
    }
    private void DisableMagToDrop()
    {
        if (slugToDrop != null)
        {
            slugToDrop.SetActive(false);
            slugToDrop = null;
            if (slugPos.childCount > 0)
            {
                slugPos.GetChild(0).gameObject.SetActive(false);
            }
            UpdateAmmo();
        }

    }

    public int GetAmmoStatus()
    {
        Slug[] slugs;

        slugs = slugPos.gameObject.GetComponentsInChildren<Slug>(true);
        return slugs.Length;
    }
    public int GetAmmoToAdd()
    {
        return ammoToAdd;
    }
}
