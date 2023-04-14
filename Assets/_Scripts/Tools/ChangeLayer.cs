using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    public float delay;
    public void NewLayer()
    {
        CancelInvoke();
        gameObject.layer = 8;
    }
    public void OriginalLayer()
    {
        Invoke(nameof(ActualChange), delay);
    }
    private void ActualChange()
    {
        gameObject.layer = 0;
    }
}
