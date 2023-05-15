using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AmmoBagShotgun : MonoBehaviour
{
    public int slugsToStartWith;
    public Transform slugPos;
    public GameObject slugPrefab;
    public TextMeshProUGUI ammoText;

    private int ammo;
    private GameObject slugToDrop;
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
    public void ReleasingSlug()
    {
        CancelInvoke();
        if (handIn && slugToDrop != null)
        {
            slugToDrop.GetComponent<Slug>().EnableGravity(false);
            slugToDrop.transform.parent = slugPos;
            slugToDrop.transform.position = slugPos.position;
            slugToDrop.transform.rotation = slugPos.rotation;
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
}
