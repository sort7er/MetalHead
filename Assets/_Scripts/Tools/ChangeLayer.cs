using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    public LayerMask newLayer;
    public void NewLayer()
    {
        gameObject.layer = 8;
    }
    public void OriginalLayer()
    {
        gameObject.layer = 0;
    }
}
