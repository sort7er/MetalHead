using UnityEngine;
using UnityEngine.EventSystems;

public class Grabables : MonoBehaviour
{
    public float followTime;
    public GameObject gameObjectToTrace;


    private Rigidbody gameObjectsRigidBody;
    private Vector3 targetPos;
    private bool left, hover;

    private void Start()
    {
        gameObjectsRigidBody = gameObjectToTrace.GetComponent<Rigidbody>();
    }
    public void Hover()
    {
        if (GameManager.instance.CheckHover(gameObject) == 1)
        {
            left = true;
            targetPos = new Vector3(transform.position.x, GameManager.instance.leftHand.transform.position.y, transform.position.z);
        }
        else if (GameManager.instance.CheckHover(gameObject) == 2)
        {
            left = false;
            targetPos = new Vector3(transform.position.x, GameManager.instance.rightHand.transform.position.y, transform.position.z);
        }
        gameObjectsRigidBody.isKinematic = true;
        gameObjectsRigidBody.useGravity = false;
        hover = true;
    }
    public void StopHovering()
    {
        gameObjectsRigidBody.isKinematic = false;
        gameObjectsRigidBody.useGravity = true;
        hover = false;
    }

    private void FixedUpdate()
    {

        transform.rotation= Quaternion.identity;
        
        if(hover)
        {
            //if(left)
            //{
            //    targetPos = new Vector3(transform.position.x, GameManager.instance.leftHand.transform.position.y, transform.position.z);
                
            //}
            //else
            //{
            //    targetPos = new Vector3(transform.position.x, GameManager.instance.rightHand.transform.position.y, transform.position.z);
            //}
            gameObjectToTrace.transform.position = Vector3.Lerp(gameObjectToTrace.transform.position, targetPos, Time.deltaTime * followTime);
        }
        else
        {
            transform.position = gameObjectToTrace.transform.position;
        }

    }
}
