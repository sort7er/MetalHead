using UnityEngine;
using UnityEngine.Events;

public class DynamicTrigger : MonoBehaviour
{
    public string tagToCompare;
    public UnityEvent enter, exit;

    public GameObject enteringGameObject;
    private bool triggerEnabled;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToCompare) && triggerEnabled)
        {
            enteringGameObject = other.gameObject;
            enter.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagToCompare) && triggerEnabled)
        {
            enteringGameObject = null;
            exit.Invoke();
        }
    }

    public void TriggerEnabled(bool state)
    {
        triggerEnabled = state;
    }

    public GameObject GetGameObject() { return enteringGameObject; }
}
