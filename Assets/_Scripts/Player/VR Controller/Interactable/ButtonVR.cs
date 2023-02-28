using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public Transform press;
    public UnityEvent onPress, onRelease;

    private MeshRenderer pressMeshRenderer;
    private bool isPressed;

    private void Start()
    {
        pressMeshRenderer = press.GetComponent<MeshRenderer>();
        pressMeshRenderer.material.EnableKeyword("_EMISSION");
        SetPressPosY(1.5f);
    }

    public void PressButton()
    {
        if (!isPressed)
        {
            isPressed = true;
            onPress.Invoke();
            pressMeshRenderer.material.DisableKeyword("_EMISSION");
            SetPressPosY(0.5f);
        }
    }

    public void ReleaseButton()
    {
        if (isPressed)
        {
            isPressed = false;
            onRelease.Invoke();
            pressMeshRenderer.material.EnableKeyword("_EMISSION");
            SetPressPosY(1.5f);
        }
    }
    public void InvokeReleaseButton(float time)
    {
        Invoke("ReleaseButton", time);
    }

    private void SetPressPosY(float y)
    {
        press.transform.localPosition = new Vector3(0, y, 0);
    }
}
