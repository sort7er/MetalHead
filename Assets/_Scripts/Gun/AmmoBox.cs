using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public float smoothTime;
    public GameObject box;

    private Vector3 targetPos;
    private AudioSource ammoBoxSource;
    private BoxCollider boxCollider;
    private bool left, pickedUp;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        ammoBoxSource = GetComponent<AudioSource>();
    }

    public void GrabBox()
    {
        if(GameManager.instance.CheckGameObject(gameObject) == 1)
        {
            left = true;
        }
        else if(GameManager.instance.CheckGameObject(gameObject) == 2)
        {
            left = false;
        }
        pickedUp = true;
    }

    private void Update()
    {
        if(pickedUp)
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
            if (Vector3.Distance(transform.position, targetPos) <= 0.05f)
            {
                pickedUp = false;
                ammoBoxSource.Play();
                box.SetActive(false);
                boxCollider.enabled = false;
                Destroy(gameObject, 0.5f);
            }

        }
    }
}
