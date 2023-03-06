using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public GameObject box;

    private AudioSource ammoBoxSource;
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        ammoBoxSource = GetComponent<AudioSource>();
    }

    public void GrabBox()
    {
        ammoBoxSource.Play();
        box.SetActive(false);
        boxCollider.enabled = false;
        Destroy(ammoBoxSource, 0.5f);
    }
}
