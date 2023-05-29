using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.rotation =  Quaternion.Euler(GameManager.instance.cam.transform.eulerAngles.x, GameManager.instance.cam.transform.eulerAngles.y, 0);
    }
}
