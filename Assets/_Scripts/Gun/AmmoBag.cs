using UnityEngine;

public class AmmoBag : MonoBehaviour
{
    public int MaxAmmo;

    public GameObject magazinePrefab;
    public Transform spawn;

    private GameObject magazine;
    public int currentTotalAmmo, rest, numberOfFullMags;

    private void Update()
    {
        rest = currentTotalAmmo % 10;
        numberOfFullMags = (currentTotalAmmo - rest) / 10;
    }
    //public void InstatntiateMagazine()
    //{
    //    magazine = Instantiate(magazinePrefab, spawn.position, Quaternion.identity);
    //    magazine.GetComponent<Rigidbody>().isKinematic = true;
    //    magazine.GetComponent<Rigidbody>().useGravity = false;
    //}
}
