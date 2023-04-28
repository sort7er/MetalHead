using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    public GameObject push;
    public MeshRenderer visual;
    public float threshold = 0.1f;
    public float deadzone = 0.025f;
    public UnityEvent onPressed, onReleased;
    public AudioClip[] buttonDown;
    public AudioClip[] buttonUp;
 
    [HideInInspector] public bool frozen;

    private bool isPressed;
    private Vector3 startPos;
    private Vector3 endPos;
    private Rigidbody rb;
    private ConfigurableJoint joint;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource= GetComponent<AudioSource>();
        rb = push.GetComponent<Rigidbody>();
        startPos = push.transform.localPosition;
        joint = push.GetComponent<ConfigurableJoint>();
        endPos = new Vector3(push.transform.localPosition.x, push.transform.localPosition.y - joint.linearLimit.limit + 0.002f, push.transform.localPosition.z);
    }

    private void Update()
    {
        if(!isPressed && GetValue() + threshold >= 1 )
        {
            Pressed();
        }
        else if(isPressed && GetValue() - threshold <= 0)
        {
            Released();
        }
        if (GetValue() > 0.9 || push.transform.localPosition.y > startPos.y + 0.002f && !frozen)
        {
            if(push.transform.localPosition.y > startPos.y)
            {
                push.transform.localPosition = startPos;
            }
            else
            {
                push.transform.localPosition = endPos;
            }


            CancelInvoke(nameof(Unfreeze));
            Invoke(nameof(Unfreeze), 3);
            rb.constraints = RigidbodyConstraints.FreezeAll;
            frozen = true;
        }
    }

    public void Unfreeze()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
        frozen = false;
    }

    private float GetValue()
    {
        var value = Vector3.Distance(startPos, push.transform.localPosition ) / joint.linearLimit.limit;
        if(Mathf.Abs(value) < deadzone)
        {
            value = 0;
        }
        return Mathf.Clamp(value, - 1, 1);
    }

    private void Pressed()
    {
        visual.material.EnableKeyword("_EMISSION");
        isPressed = true;
        onPressed.Invoke();
        audioSource.clip = buttonDown[Random.Range(0, buttonDown.Length)];
        audioSource.Play();
    }
    private void Released()
    {
        visual.material.DisableKeyword("_EMISSION");
        isPressed = false;
        onReleased.Invoke();
        audioSource.clip = buttonUp[Random.Range(0, buttonUp.Length)];
        audioSource.Play();
    }
}
