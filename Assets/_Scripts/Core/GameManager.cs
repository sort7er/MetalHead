using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject gameOverCanvas;

    public Animator tempAnim;
    public GameObject[] enemies;


    [HideInInspector] public bool isUpgrading;
    [HideInInspector] public bool isPaused;
    [HideInInspector] public bool isDead;

    private int tempNumber;
    private bool tempDone;
    private PauseMenu pauseMenu;
    private XRInteractorLineVisual leftLineVisual, rightLineVisual;
    private XRDirectInteractor rHand, lHand;

    private void Start()
    {
        tempDone= false;
        Physics.IgnoreLayerCollision(7, 8);
        EnableRays(false);
        pauseMenu = GetComponent<PauseMenu>();
        leftLineVisual = leftRayInteractor.GetComponent<XRInteractorLineVisual>();
        rightLineVisual = rightRayInteractor.GetComponent<XRInteractorLineVisual>();
        lHand = leftHand.gameObject.GetComponent<XRDirectInteractor>();
        rHand = rightHand.gameObject.GetComponent<XRDirectInteractor>();
        SetTimeScale(1);
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
        if(lHand != null)
        {
            lHand.enabled = state;
        }
        if(rHand != null)
        {
            rHand.enabled = state;
        }
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
        if(!isUpgrading && isPaused)
        {
            EnableRays(true);
            EnableDirectInteractors(!false);
        }
        else
        {
            EnableRays(state);
            EnableDirectInteractors(!state);
        }
        
    }
    public void IsPaused(bool state)
    {
        isPaused = state;
    }
    public void SetTimeScale(float newTime)
    {
        Time.timeScale = newTime;
    }
    public void IsDead()
    {
        isDead = true;
        Invoke("DeadMenu", 0.3f);
        EnableRays(true);
        EnableDirectInteractors(false);
        LocomotionManager.instance.EnableMovement(false);
        LocomotionManager.instance.EnableTurning(false);

    }
    private void DeadMenu()
    {
        if(pauseMenu != null)
        {
            pauseMenu.FollowCam(false);
        }
        gameOverCanvas.SetActive(true);
        SetTimeScale(0f);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void TempAddOne()
    {
        if (!tempDone)
        {
            tempNumber++;
            if (tempNumber >= 5)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].SetActive(true);
                }
                tempAnim.SetTrigger("Open");
                tempDone = true;
            }
        }
    }
}
