using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public AmmoBagShotgun ammoBagShotgun;
    public Magnet magnet;
    public GameObject gameOverCanvas;
    public XRDirectInteractor rHand, lHand;
    public InputActionAsset gripInput;
    public Gate[] gates;
    public AudioSource[] soundsToSlowDown;
    public AudioClip[] slowDownSound;

    [HideInInspector] public bool isUpgrading;
    [HideInInspector] public bool isPaused;
    [HideInInspector] public bool isDead;

    private int waitingID, currentID;
    private PauseMenu pauseMenu;
    private XRInteractorLineVisual leftLineVisual, rightLineVisual;
    private InputAction gripLeft, gripRight;
    private AudioSource audioSource;
    private float leftGripValue, rightGripValue;
    private bool ammoBagTaken;


    private float targetTime;
    private bool smoothTimeScale;

    private void Start()
    {
        Physics.IgnoreLayerCollision(7, 8);
        EnableRays(false);
        audioSource = GetComponent<AudioSource>();
        pauseMenu = GetComponent<PauseMenu>();
        leftLineVisual = leftRayInteractor.GetComponent<XRInteractorLineVisual>();
        rightLineVisual = rightRayInteractor.GetComponent<XRInteractorLineVisual>();
        lHand = leftHand.gameObject.GetComponent<XRDirectInteractor>();
        rHand = rightHand.gameObject.GetComponent<XRDirectInteractor>();
        SetTimeScale(1);
        rightHand.questController.QuestActive(false);
        leftHand.questController.QuestActive(false);

    }
        private void OnEnable()
    {
        gripLeft = gripInput.FindActionMap("XRI LeftHand Interaction").FindAction("Select Value");
        gripLeft.Enable();
        gripLeft.performed += SelecetLeft;

        gripRight = gripInput.FindActionMap("XRI RightHand Interaction").FindAction("Select Value");
        gripRight.Enable();
        gripRight.performed += SelecetRight;
    }
    private void OnDisable()
    {
        gripLeft.performed -= SelecetLeft;
        gripRight.performed -= SelecetRight;
    }
    private void Update()
    {
        if (!lHand.allowSelect)
        {
            if(leftGripValue < 0.5f)
            {
                TurnOnLeftInteractor();
            }
        }
        if (!rHand.allowSelect)
        {
            if (rightGripValue < 0.5f)
            {
                TurnOnRightInteractor();
            }
        }
        if (smoothTimeScale)
        {
            if(targetTime > Time.timeScale)
            {
                Time.timeScale += Time.unscaledDeltaTime * 0.35f;
                RenderSettings.fogDensity -= Time.unscaledDeltaTime * 0.15f;
                for(int i = 0; i < soundsToSlowDown.Length; i++)
                {
                    if (soundsToSlowDown[i] != null)
                    {
                        soundsToSlowDown[i].pitch += Time.unscaledDeltaTime * 0.3f;
                    }
                }
                
                if (Time.timeScale >= targetTime)
                {
                    RenderSettings.fogDensity = 0;
                    RenderSettings.fog = false;
                    Time.timeScale = targetTime;
                    Time.fixedDeltaTime= Time.timeScale * 0.02f;
                    for (int i = 0; i < soundsToSlowDown.Length; i++)
                    {
                        if (soundsToSlowDown[i] != null)
                        {
                            soundsToSlowDown[i].pitch = 1;
                        }
                    }
                    smoothTimeScale = false;
                }
            }
            else
            {
                Time.timeScale -= Time.unscaledDeltaTime * 0.35f;
                RenderSettings.fogDensity += Time.unscaledDeltaTime * 0.15f;
                for (int i = 0; i < soundsToSlowDown.Length; i++)
                {
                    if (soundsToSlowDown[i] != null)
                    {
                        soundsToSlowDown[i].pitch -= Time.unscaledDeltaTime * 0.3f;
                    }
                }
                if (Time.timeScale <= targetTime)
                {
                    for (int i = 0; i < soundsToSlowDown.Length; i++)
                    {
                        if (soundsToSlowDown[i] != null)
                        {
                            soundsToSlowDown[i].pitch = 0.6f;
                        }
                    }
                    
                    RenderSettings.fogDensity = 0.3f;
                    Time.timeScale = targetTime;
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                    smoothTimeScale = false;
                }
            }
        }

    }

    private void SelecetLeft(InputAction.CallbackContext context)
    {
        leftGripValue = context.ReadValue<float>();
    }
    private void SelecetRight(InputAction.CallbackContext context)
    {
        rightGripValue = context.ReadValue<float>();
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

    public void EnableLeftInteractor(bool state)
    {
        if (lHand != null)
        {
            if (!state)
            {
                leftHand.HoverDone();
            }
            lHand.enabled = state;
        }
    }
    public void EnableRightInteractor(bool state)
    {
        if (rHand != null)
        {
            if (!state)
            {
                rightHand.HoverDone();
            }

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
    public void SetTimeScaleSmooth(float newTime)
    {
        smoothTimeScale = true;
        targetTime = newTime;
        if(newTime < Time.timeScale)
        {
            audioSource.clip = slowDownSound[0];
            audioSource.Play();
            RenderSettings.fog = true;
            RenderSettings.fogDensity = 0;
        }
        else
        {
            audioSource.clip = slowDownSound[1];
            audioSource.Play();
        }
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

    public void TurnOffLeftInteractor()
    {
        lHand.allowSelect = false;
    }
    public void TurnOffRightInteractor()
    {
        rHand.allowSelect = false;
    }

    public void TurnOnLeftInteractor()
    {
        lHand.allowSelect = true;
    }
    public void TurnOnRightInteractor()
    {
        rHand.allowSelect = true;
    }
    public void CheckCurrentAmmoBag(int weaponID)
    {
        if (!ammoBagTaken)
        {
            AmmoBagToEnable(weaponID);
            currentID = weaponID;
            if (currentID != 0)
            {
                ammoBagTaken= true;
            }
        }
        else
        {
            waitingID = weaponID;
        }
    }
    public void ReleaseWeapon(int weaponID)
    {
        if(weaponID == currentID)
        {
            ammoBagTaken = false;
            CheckCurrentAmmoBag(waitingID);
            waitingID = 0;
        }
        else if(weaponID == waitingID)
        {
            waitingID = 0;
        }
    }

    private void AmmoBagToEnable(int weaponID)
    {
        if(weaponID == 1)
        {
            ammoBag.gameObject.SetActive(true);
            ammoBagShotgun.gameObject.SetActive(false);
        }
        else if(weaponID == 2)
        {
            ammoBag.gameObject.SetActive(false);
            ammoBagShotgun.gameObject.SetActive(true);
        }
        else
        {
            ammoBag.gameObject.SetActive(false);
            ammoBagShotgun.gameObject.SetActive(false);
        }
    }


    public void CheckWithGates()
    {
        for (int i = 0; i < gates.Length; i++)
        {
            gates[i].CheckIfDead();
        }
    }
}
