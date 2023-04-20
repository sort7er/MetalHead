using UnityEngine;

public class Hover : MonoBehaviour
{
    public Outline outline;

    private void Start()
    {
        outline.enabled = false;
    }

    public void Hovering(bool state)
    {
        if (state)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }
}
