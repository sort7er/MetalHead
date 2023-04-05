using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.rotation =  Quaternion.Euler(0, GameManager.instance.cam.transform.eulerAngles.y, 0);
    }
}
