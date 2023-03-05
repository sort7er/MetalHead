using UnityEngine;
using UnityEngine.Events;

public class DynamicTrigger : MonoBehaviour
{
    public bool compareColliders;
    public Collider[] collidersToCompare;

    public string tagToCompare;
    public UnityEvent enter, exit;

    private bool triggerDisabled;

    private GameObject enteringGameObject;


    private void OnTriggerEnter(Collider other)
    {
        if (!triggerDisabled)
        {
            if (compareColliders)
            {
                for(int i = 0; i < collidersToCompare.Length; i++)
                {
                    if(other == collidersToCompare[i])
                    {
                        enter.Invoke();
                        break;
                    }
                }
            }
            else
            {
                if (other.CompareTag(tagToCompare))
                {
                    enteringGameObject = other.gameObject;
                    enter.Invoke();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!triggerDisabled)
        {
            if (compareColliders)
            {
                for (int i = 0; i < collidersToCompare.Length; i++)
                {
                    if (other == collidersToCompare[i])
                    {
                        exit.Invoke();
                        break;
                    }
                }
            }
            else
            {
                if (other.CompareTag(tagToCompare))
                {
                    enteringGameObject = null;
                    exit.Invoke();
                }
            }
        }
    }

    public void TriggerDisabled(bool state)
    {
        triggerDisabled = state;
    }

    public GameObject GetGameObject() { return enteringGameObject; }
}
