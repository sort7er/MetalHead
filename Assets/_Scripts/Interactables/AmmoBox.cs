using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static RunningEnemy;

public class AmmoBox : MonoBehaviour
{
    public TypeOfAmmo ammoType;
    public float startSmoothTime;
    public int numberOfAmmoToAdd = 10;

    private float smoothTime;
    private GameObject box;
    private Vector3 targetPos;
    private AudioSource ammoBoxSource;
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private XRGrabInteractable interactable;
    private bool left, pickedUp, grabbed;

    public enum TypeOfAmmo { Pistol, Shotgun}

    private void Start()
    {
        box = transform.GetChild(0).gameObject;
        smoothTime = startSmoothTime;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        ammoBoxSource = GetComponent<AudioSource>();
        interactable = GetComponent<XRGrabInteractable>();
    }

    public void GrabBox()
    {
        if(!grabbed)
        {
            if (GameManager.instance.CheckGameObject(gameObject) == 1)
            {
                left = true;
            }
            else if (GameManager.instance.CheckGameObject(gameObject) == 2)
            {
                left = false;
            }
            rb.useGravity = false;
            rb.isKinematic = true;
            grabbed = true;
        }
    }

    private void Update()
    {
        if(grabbed)
        {
            if(left)
            {
                targetPos = GameManager.instance.leftHand.transform.position;
            }
            else
            {
                targetPos = GameManager.instance.rightHand.transform.position;
            }

            smoothTime += 0.2f;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, smoothTime * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) <= 0.05f && !pickedUp)
            {
                
                ammoBoxSource.Play();
                interactable.enabled = false;
                

                switch (ammoType)
                {
                    case TypeOfAmmo.Pistol:
                        EffectManager.instance.SpawnMessage("+ " + numberOfAmmoToAdd.ToString() + " bullets", 1);
                        GameManager.instance.ammoBag.AddAmmo(numberOfAmmoToAdd);
                        break;
                    case TypeOfAmmo.Shotgun:
                        EffectManager.instance.SpawnMessage("+ " + numberOfAmmoToAdd.ToString() + " slugs", 1);
                        GameManager.instance.ammoBagShotgun.AddAmmo(numberOfAmmoToAdd);
                        break;
                }

                box.SetActive(false);
                boxCollider.enabled = false;
                pickedUp = true;
                Invoke(nameof(Disable), 0.5f);
            }

        }
    }
    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
