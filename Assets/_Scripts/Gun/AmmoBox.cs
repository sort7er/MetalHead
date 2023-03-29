using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public float smoothTime;
    public GameObject box;
    public int numberOfBulletsToAdd = 10;

    private Vector3 targetPos;
    private AudioSource ammoBoxSource;
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private bool left, pickedUp, grabbed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        ammoBoxSource = GetComponent<AudioSource>();
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

            transform.position = Vector3.MoveTowards(transform.position, targetPos, smoothTime * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) <= 0.05f && !pickedUp)
            {
                ammoBoxSource.Play();
                GameManager.instance.ammoBag.AddAmmo(numberOfBulletsToAdd);
                box.SetActive(false);
                boxCollider.enabled = false;
                pickedUp = true;
                Destroy(gameObject, 0.5f);
            }

        }
    }
}
