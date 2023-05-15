using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public Transform menu;
    public Transform tutorialMenu;
    public Transform menuPivot;
    public InputActionAsset menuInputAction;
    public GameObject pauseVignette;
    public GameObject exitTutorialMenu;


    [Header("Things to enable")]
    public GameObject magnet;


    private Watch watch;
    private Renderer pauseVignetteRenderer;
    private InputAction menuPressed;
    private bool followCam, canPause;
    private bool watchFound;
    private bool enableTurning, enableMovement;
    private bool canExitTutorial;
    private int currentMenu;

    private void Start()
    {
        pauseVignetteRenderer = pauseVignette.GetComponent<Renderer>();
        foreach (Transform t in menu)
        {
            t.gameObject.SetActive(false);
        }
        foreach (Transform t in tutorialMenu)
        {
            t.gameObject.SetActive(false);
        }
        PauseVignette(0);
        Invoke(nameof(StartTutorial), 0.1f);
        CanExitTutorial(true);
    }

    private void OnEnable()
    {
        menuPressed = menuInputAction.FindActionMap("XRI LeftHand Interaction").FindAction("Menu");
        menuPressed.Enable();
        menuPressed.performed += MenuPressed;
    }

    private void OnDisable()
    {
        menuPressed.performed -= MenuPressed;

    }

    private void Update()
    {
        if (followCam)
        {
            menuPivot.rotation = Quaternion.Euler(0f, GameManager.instance.cam.transform.eulerAngles.y, 0f);
        }

        if (watch == null)
        {
            watch = FindObjectOfType<Watch>();
            watchFound = false;
        }
        else
        {
            if (!watchFound)
            {
                watchFound = true;
                SetWatch(false);
            }
        }
    }

    private void MenuPressed(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.isDead && canPause)
        {
            if (!GameManager.instance.isPaused)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }
    private void StartTutorial()
    {
        TutorialMenu();
        SetMagnet(false);
    }
    public void NextMenu()
    {
        foreach (Transform t in tutorialMenu)
        {
            t.gameObject.SetActive(false);
        }
        TutorialMenu();
    }


    //Menus
    public void TutorialMenu()
    {
        OpenMenuCommon();
        tutorialMenu.GetChild(currentMenu).gameObject.SetActive(true);
        currentMenu++;
        canPause = false;
    }
    public void CloseTutorialMenu()
    {
        foreach (Transform t in tutorialMenu)
        {
            t.gameObject.SetActive(false);
        }
        canPause = true;
        CloseMenuCommon();
    }
    public void OpenMenu()
    {
        GameManager.instance.SetTimeScale(0);
        menu.GetChild(0).gameObject.SetActive(true);
        OpenMenuCommon();
    }
    public void CloseMenu()
    {
        foreach (Transform t in menu)
        {
            t.gameObject.SetActive(false);
        }
        GameManager.instance.SetTimeScale(1);
        CloseMenuCommon();
    }
    public void ExitTutorial()
    {
        if(canExitTutorial)
        {
            exitTutorialMenu.SetActive(true);
            OpenMenuCommon();
            canPause = false;
            CanExitTutorial(false);
        }

    }
    public void CanExitTutorial(bool state)
    {
        canExitTutorial = state;
    }


    //Methods
    public void FollowCam(bool state)
    {
        followCam = state;
    }
    public void PauseVignette(float newSize)
    {
        pauseVignetteRenderer.sharedMaterial.SetFloat("_ApertureSize", newSize);
    }
    public void SetWatch(bool state)
    {
        if(watch != null)
        {
            watch.gameObject.SetActive(state);
        }
    }
    public void SetMagnet(bool state)
    {
        magnet.SetActive(state);
    }
    public void SetMovement(bool state)
    {
        enableMovement = state;
        LocomotionManager.instance.EnableMovement(state);
    }
    public void SetTurning(bool state)
    {
        enableTurning = state;
        LocomotionManager.instance.EnableTurning(state);
    }


    private void OpenMenuCommon()
    {
        PauseVignette(0);
        GameManager.instance.EnableRays(true);
        GameManager.instance.EnableDirectInteractors(false);
        GameManager.instance.IsPaused(true);
        LocomotionManager.instance.EnableMovement(false);
        LocomotionManager.instance.EnableTurning(false);
        AudioManager.instance.MuteSounds();
        FollowCam(false);
    }
    private void CloseMenuCommon()
    {
        GameManager.instance.IsPaused(false);
        GameManager.instance.EnableRays(false);
        GameManager.instance.EnableDirectInteractors(true);
        AudioManager.instance.Unmute();
        FollowCam(true);
        PauseVignette(1);
        if (enableMovement)
        {
            LocomotionManager.instance.EnableMovement(true);
        }
        if (enableTurning)
        {
            LocomotionManager.instance.EnableTurning(true);
        }
    }
}
