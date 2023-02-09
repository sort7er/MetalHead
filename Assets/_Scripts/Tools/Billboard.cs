using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.rotation = GameManager.instance.cam.transform.rotation;
    }
}
