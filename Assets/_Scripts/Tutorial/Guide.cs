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

    private Vector3 camPosition, Xoffset, Yoffset;
    private Quaternion camRotation;
    private Transform cam, targetTransform;
    private LineRenderer lineRenderer;
    private Vector3 currentEdge, currentCorner;
    private int currentY;

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
        camPosition = cam.position + cam.forward + Xoffset + Yoffset;
        camRotation = Quaternion.Euler(cam.eulerAngles.x, cam.eulerAngles.y, guideCanvas.eulerAngles.z);
        guideCanvas.position = Vector3.Lerp(guideCanvas.position, camPosition, Time.deltaTime * 10);
        guideCanvas.rotation = Quaternion.Slerp(guideCanvas.rotation, camRotation, Time.unscaledDeltaTime * 10);



        //Line
        if(targetTransform != null)
        {
            Vector3 prep = Vector3.Cross(cam.forward, targetTransform.position - cam.position);

            if (Vector3.Dot(prep, Vector3.up) < 0)
            {
                Xoffset = -cam.right * Xamount;
                currentEdge = leftEdge.position;
                currentCorner = leftCorner.position;
            }
            else
            {
                Xoffset = cam.right * Xamount;
                currentEdge = rightEdge.position;
                currentCorner = rightCorner.position;
            }

            if (currentY == 1)
            {
                Yoffset = cam.up * Yamount;
            }
            else if (currentY == 2)
            {
                Yoffset = Vector3.zero;
            }
            else
            {
                Yoffset = cam.up * Yamount;
            }


            lineRenderer.SetPosition(0, currentEdge);
            lineRenderer.SetPosition(1, currentCorner);
            lineRenderer.SetPosition(2, targetTransform.position);
        }

    }

    public void SetGuide(int y, Transform target, string messageText)
    {
        CancelInvoke(nameof(Close));
        guideCanvas.gameObject.SetActive(true);

        if (!guideAnim.GetBool("Open"))
        {
            guideAnim.SetBool("Open", true);
        }


        currentY = y;
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
