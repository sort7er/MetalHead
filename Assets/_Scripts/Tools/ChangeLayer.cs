using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    public void NewLayer()
    {
        gameObject.layer = 8;
    }
    public void OriginalLayer()
    {
        gameObject.layer = 0;
    }
}
