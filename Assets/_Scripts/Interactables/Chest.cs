using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Chest : MonoBehaviour
{
    public GameObject treasureToSpawn;
    public Transform tresurePosition;
    public GameObject key;

    private Rigidbody keyrb;
    private Collider keyCollider;
    private SphereCollider keyTrigger;
    private XRSocketInteractor keyHole;
    private Animator chestAnim;
    public bool opened, keyObtained, holdingKey, insideTrigger;

    private void Start()
    {
        keyrb = key.GetComponent<Rigidbody>();
        keyCollider = key.GetComponent<Collider>();
        keyTrigger = key.GetComponentInChildren<SphereCollider>();
        keyHole = GetComponentInChildren<XRSocketInteractor>();
        chestAnim= GetComponent<Animator>();
        HideKey();
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, GameManager.instance.XROrigin.transform.position) < 2f)
        {
            if (!insideTrigger)
            {
                Hover();
                insideTrigger = true;
            }
        }
        else
        {
            if (insideTrigger)
            {
                HoverDone();
                insideTrigger = false;
            }
        }
    }

    public void Hover()
    {
        CancelInvoke();
        if (keyObtained)
        {
            chestAnim.SetBool("Key", true);
            key.SetActive(true);
        }
    }
    public void HoverDone()
    {
        if (keyObtained && !opened && !holdingKey)
        {
            key.transform.localPosition = Vector3.zero;
            chestAnim.SetBool("Key", false);
            Invoke(nameof(HideKey), 0.5f);
        }
    }
    private void HideKey()
    {

        key.SetActive(false);
        keyrb.useGravity = false;
        keyrb.isKinematic = true;
    }
    public void HoldingKey(bool state)
    {
        holdingKey = state;
        if(!holdingKey)
        {
            HoverDone();
        }
    }
    public void Open()
    {
        if (!opened)
        {
            keyCollider.enabled = false;
            keyTrigger.enabled = false;
            chestAnim.SetTrigger("Open");
            Invoke(nameof(SpawnPickUps), 1.1f);
            opened = true;
            Instantiate(treasureToSpawn, tresurePosition.position, Quaternion.identity, tresurePosition);
        }
    }

    private void SpawnPickUps()
    {
        EffectManager.instance.SpawnPickups(transform, 10);
    }
}
