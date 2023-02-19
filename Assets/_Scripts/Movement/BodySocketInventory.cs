using UnityEngine;

[System.Serializable]
public class BodySocket
{
    public GameObject gameObject;
    [Range(0.01f, 1f)]
    public float heightRatio;
}

public class BodySocketInventory : MonoBehaviour
{

    public BodySocket[] bodySockets;
    public float heightOfPlayer;

    private Transform HMD;
    private Vector3 currentHMDPos;
    private Quaternion currentHMDRot;

    private void Start()
    {
        HMD = GameManager.instance.cam.transform;
    }


    private void Update()
    {
        currentHMDPos = HMD.position;
        currentHMDRot = HMD.rotation;
        foreach(var bodySocket in bodySockets)
        {
            UpdateHeight(bodySocket);
        }
        UpdateInventory();
    }
    private void UpdateHeight(BodySocket bodySocket)
    {
        bodySocket.gameObject.transform.position = new Vector3(bodySocket.gameObject.transform.position.x, HMD.position.y - heightOfPlayer * bodySocket.heightRatio, bodySocket.gameObject.transform.position.z);
    }
    private void UpdateInventory()
    {
        transform.position = new Vector3(currentHMDPos.x, 0 , currentHMDPos.z);
        transform.rotation = new Quaternion(transform.rotation.x, currentHMDRot.y, transform.rotation.z, currentHMDRot.w);
    }
}
