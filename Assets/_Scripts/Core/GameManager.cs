using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject cam;
    public GameObject XROrigin;
    public GameObject leftRayInteractor, rightRayInteractor;
    public Hand leftHand, rightHand;
    public AmmoBag ammoBag;
    public Magnet magnet;
    [HideInInspector] public bool isUpgrading;

    private XRInteractorLineVisual leftLineVisual, rightLineVisual;
    private XRDirectInteractor rHand, lHand;

    private void Start()
    {
        Physics.IgnoreLayerCollision(7, 8);
        EnableRays(false);
        leftLineVisual = leftRayInteractor.GetComponent<XRInteractorLineVisual>();
        rightLineVisual = rightRayInteractor.GetComponent<XRInteractorLineVisual>();
        lHand = GameManager.instance.leftHand.gameObject.GetComponent<XRDirectInteractor>();
        rHand = GameManager.instance.rightHand.gameObject.GetComponent<XRDirectInteractor>();
    }

    public void EnableRays(bool state)
    {
        leftRayInteractor.SetActive(state);
        rightRayInteractor.SetActive(state);
        leftHand.UsingRay(state);
        rightHand.UsingRay(state);
        if (leftLineVisual != null)
        {
            leftLineVisual.reticle.SetActive(state);
        }
        if (rightLineVisual != null)
        {
            rightLineVisual.reticle.SetActive(state);
        }
    }
    public void EnableDirectInteractors(bool state)
    {
        lHand.enabled = state;
        rHand.enabled = state;
    }
    public void SetXROriginRotation(Transform newRotation)
    {
        XROrigin.transform.rotation = Quaternion.Euler(0, newRotation.eulerAngles.y - cam.transform.localEulerAngles.y, 0);
    }

    public int CheckHand(string tagToCompare)
    {
        if (lHand.GetOldestInteractableSelected() != null && lHand.GetOldestInteractableSelected().transform.CompareTag(tagToCompare))
        {
            return 1;
        }
        else if (rHand.GetOldestInteractableSelected() != null && rHand.GetOldestInteractableSelected().transform.CompareTag(tagToCompare))
        {
            return 2;
        }
        else
        {
            return 0; 
        }
    }
    public int CheckGameObject(GameObject gameObjectToCheck)
    {
        if (lHand.GetOldestInteractableSelected() != null && lHand.GetOldestInteractableSelected().transform.gameObject == gameObjectToCheck)
        {
            return 1;
        }
        else if (rHand.GetOldestInteractableSelected() != null && rHand.GetOldestInteractableSelected().transform.gameObject == gameObjectToCheck)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
    public int CheckHover(GameObject gameObjectToCheck)
    {
        if (lHand.GetOldestInteractableHovered() != null && lHand.GetOldestInteractableHovered().transform.gameObject == gameObjectToCheck)
        {
            return 1;
        }
        else if (rHand.GetOldestInteractableHovered() != null && rHand.GetOldestInteractableHovered().transform.gameObject == gameObjectToCheck)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
    public void IsUpgrading(bool state)
    {
        isUpgrading = state;
        EnableRays(state);
        EnableDirectInteractors(!state);
    }
}
