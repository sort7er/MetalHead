using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public Transform menuPivot;
    public Image camImage;
    public Color pauseColor;
    public InputActionAsset menuInputAction;

    private InputAction menuPressed;
    private Color defaulColor;
    private bool followCam = true;

    private void Start()
    {
        defaulColor = camImage.color;
        menu.SetActive(false);
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

    private void MenuPressed(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.isDead)
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
    public void OpenMenu()
    {
        RenderSettings.fog = true;
        GameManager.instance.SetTargetTimeScale(0);
        GameManager.instance.EnableRays(true);
        GameManager.instance.EnableDirectInteractors(false);
        LocomotionManager.instance.EnableMovement(false);
        LocomotionManager.instance.EnableTurning(false);
        menu.SetActive(true);
        GameManager.instance.IsPaused(true);
        followCam= false;
        camImage.color = pauseColor;
    }
    public void CloseMenu()
    {
        menu.SetActive(false);
        GameManager.instance.SetTargetTimeScale(1);
        GameManager.instance.IsPaused(false);
        followCam = true;
        if(!GameManager.instance.isUpgrading)
        {
            GameManager.instance.EnableRays(false);
            GameManager.instance.EnableDirectInteractors(true);
        }
        LocomotionManager.instance.EnableMovement(true);
        LocomotionManager.instance.EnableTurning(true);
        RenderSettings.fog = false;
        camImage.color = defaulColor;
    }


    private void Update()
    {
        if(followCam)
        {
            menuPivot.rotation = Quaternion.Euler(0f, GameManager.instance.cam.transform.eulerAngles.y, 0f);
        }
    }
}
