using UnityEngine;

public class ReturnToHolster : MonoBehaviour
{

    public float timeUntilHolster, startSmoothTime;
    public Transform holster;

    [HideInInspector] public bool isHolding, isHolstered;

    private Vector3 velocity;
    private Rigidbody rb;
    private float timer, smoothTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        smoothTime = startSmoothTime;
    }

    private void Update()
    {
        if (!isHolding && !isHolstered)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Return();
                smoothTime += 0.2f;
            }
        }
    }

    public void IsHolding()
    {
        isHolding = true;
    }
    public void Release()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        isHolding = false;
        isHolstered = false;
        timer = timeUntilHolster;
        transform.parent = null;
    }
    private void Return()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.position = Vector3.MoveTowards(transform.position, holster.position, smoothTime * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, holster.rotation, smoothTime * Time.deltaTime);
        if (Vector3.Distance(transform.position, holster.position) < 0.05f)
        {
            isHolstered = true;
            transform.parent = holster;
            transform.position = holster.position;
            transform.rotation = holster.rotation;
            smoothTime = startSmoothTime;
        }
    }

}
