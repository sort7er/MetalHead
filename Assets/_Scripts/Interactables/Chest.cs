using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Chest : MonoBehaviour
{
    public float smoothTime;
    public GameObject treasureToSpawn;
    public Transform tresurePosition, keySlot, leftAttach, rightAttach;
    public GameObject key, canvas;


    private RotateObject rotateObject;
    private DynamicTrigger dynamicTrigger;
    private Rigidbody keyrb;
    private XRGrabInteractable keyInteractable, treasueInteractable;
    private Animator chestAnim;
    private bool opened, holdingKey, insideTrigger, lerpBack;

    private void Start()
    {
        rotateObject = GetComponentInChildren<RotateObject>();
        dynamicTrigger = GetComponentInChildren<DynamicTrigger>();
        keyrb = key.GetComponent<Rigidbody>();
        keyInteractable = key.GetComponent<XRGrabInteractable>();
        chestAnim = GetComponent<Animator>();
        HideKey();
        rotateObject.enabled = true;
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, GameManager.instance.XROrigin.transform.position) < 2f)
        {
            if (!insideTrigger)
            {
                Inside();
                insideTrigger = true;
            }
        }
        else
        {
            if (insideTrigger)
            {
                Outside();
                insideTrigger = false;
            }
        }
        if(lerpBack)
        {
            key.transform.localPosition = Vector3.Lerp(key.transform.localPosition, Vector3.zero, Time.deltaTime * smoothTime);
            if(Vector3.Distance(key.transform.localPosition, Vector3.zero) < 0.2f)
            {
                lerpBack = false;
                key.transform.localPosition = Vector3.zero;
                rotateObject.enabled = true;
                if (!insideTrigger)
                {
                    StartHidingKey();
                }
            }
        }
    }

    public void Inside()
    {
        CancelInvoke(nameof(HideKey));
        if (GameManager.instance.AmountOfKeys() > 0)
        {
            key.SetActive(true);
            
        }
        else
        {
            canvas.SetActive(true);
        }
        chestAnim.SetBool("Key", true);
        Invoke(nameof(StartShowingKey), 0.5f);
    }
    public void Outside()
    {
        if (!opened && !holdingKey && !lerpBack)
        {
            StartHidingKey();
        }
    }
    private void StartShowingKey()
    {
        if(!opened)
        {
            keyInteractable.enabled = true;
        }
    }
    private void StartHidingKey()
    {
        keyInteractable.enabled= false;
        chestAnim.SetBool("Key", false);
        Invoke(nameof(HideKey), 0.5f);
    }

    private void HideKey()
    {
        key.transform.localPosition = Vector3.zero;
        key.SetActive(false);
        rotateObject.enabled = true;
        keyrb.useGravity = false;
        keyrb.isKinematic = true;
        canvas.SetActive(false);
    }
    public void GrabKey()
    {
        if (GameManager.instance.CheckGameObject(key) == 1)
        {
            keyInteractable.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabWrench(true);
        }
        if (GameManager.instance.CheckGameObject(key) == 2)
        {
            keyInteractable.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabWrench(true);
        }

        holdingKey = true;
        rotateObject.enabled = false;
    }
    public void ReleaseKey()
    {
        GameManager.instance.leftHand.GrabWrench(false);
        GameManager.instance.rightHand.GrabWrench(false);
        holdingKey = false;
        Invoke(nameof(CheckIfInChest), 0.05f);
    }
    private void CheckIfInChest()
    {
        if (!holdingKey)
        {
            lerpBack = true;
        }
    }

    public void Open()
    {
        if (!opened)
        {
            GameManager.instance.GotAKey(false);
            keyInteractable.enabled = false;
            key.transform.parent = keySlot;
            key.transform.position = keySlot.position;
            key.transform.rotation = Quaternion.Euler(0,180,0);
            chestAnim.SetTrigger("Open");
            EffectManager.instance.Key(transform.position, 1);
            Invoke(nameof(SpawnPickUps), 1.1f);
            opened = true;
            holdingKey = true;
            GameObject ring = Instantiate(treasureToSpawn, tresurePosition.position, Quaternion.identity, tresurePosition);
            treasueInteractable = ring.GetComponent<XRGrabInteractable>();
            treasueInteractable.enabled = false;
        }
    }

    private void SpawnPickUps()
    {
        EffectManager.instance.SpawnPickups(transform, 10, true);
        treasueInteractable.enabled = true;
        
    }
}
