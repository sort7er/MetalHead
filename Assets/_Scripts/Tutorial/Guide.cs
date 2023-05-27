using TMPro;
using UnityEngine;

public class Guide : MonoBehaviour
{
    public static Guide instance;
    public Transform guideCanvas;
    public Transform leftEdge, leftCorner;
    public Transform rightEdge, rightCorner;

    private Animator guideAnim;

    private void Awake()
    {
        instance = this;
    }

    public float Xamount, Yamount;
    public TextMeshProUGUI message;

    private Vector3 camPosition, offset;
    private Quaternion camRotation;
    private Transform cam, targetTransform;
    private LineRenderer lineRenderer;

    private Vector3 currentEdge, currentCorner;
    private bool left;

    private void Start()
    {
        guideAnim = GetComponent<Animator>();
        lineRenderer = guideCanvas.GetComponent<LineRenderer>();
        GuideDone();
    }

    void Update()
    {

        cam = GameManager.instance.cam.transform;

        //Position and rotation of message
        camPosition = cam.position + cam.forward + offset;
        camRotation = Quaternion.Euler(cam.eulerAngles.x, cam.eulerAngles.y, guideCanvas.eulerAngles.z);
        guideCanvas.position = Vector3.Lerp(guideCanvas.position, camPosition, Time.deltaTime * 5);
        guideCanvas.rotation = Quaternion.Slerp(guideCanvas.rotation, camRotation, Time.deltaTime * 5);


        //Line
        if(left)
        {
            currentEdge = leftEdge.position;
            currentCorner = leftCorner.position;
        }
        else
        {
            currentEdge = rightEdge.position;
            currentCorner = rightCorner.position;
        }
        lineRenderer.SetPosition(0, currentEdge);
        lineRenderer.SetPosition(1, currentCorner);
        if(targetTransform!= null)
        {
            lineRenderer.SetPosition(2, targetTransform.position);
        }

    }

    public void SetGuide(int x, int y, Transform target, string messageText)
    {
        CancelInvoke(nameof(Close));
        guideCanvas.gameObject.SetActive(true);

        if (!guideAnim.GetBool("Open"))
        {
            guideAnim.SetBool("Open", true);
        }

        if(x == 1)
        {
            offset = -cam.right * Xamount;
            left = true;
        }
        else
        {
            offset = cam.right * Xamount;
            left = false;
        }

        if(y == 1)
        {
            offset -= cam.up * Yamount;
        }
        else if (y == 2)
        {
            offset += Vector3.zero;
        }
        else 
        {
            offset += cam.up * Yamount;
        }

        message.text = messageText;
        targetTransform = target;
    }
    public void GuideDone()
    {
        guideAnim.SetBool("Open", false);
        Invoke(nameof(Close), 0.5f);
    }
    private void Close()
    {
        guideCanvas.gameObject.SetActive(false);
    }
}
