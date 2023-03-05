using UnityEngine;
using UnityEngine.Events;

public class DynamicCollision : MonoBehaviour
{
    public bool compareTag;

    public string tagToCompare;
    public UnityEvent enter, exit;


    private void OnCollisionEnter(Collision collision)
    {
        if (compareTag)
        {
            if (collision.transform.CompareTag(tagToCompare))
            {
                enter.Invoke();
            }
        }
        else
        {
            enter.Invoke();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (compareTag)
        {
            if (collision.transform.CompareTag(tagToCompare))
            {
                exit.Invoke();
            }
        }
        else
        {
            exit.Invoke();
        }
    }
}
