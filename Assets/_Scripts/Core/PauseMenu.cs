using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Transform menu;
    public Transform menuPivot;
    public Image camImage;
    public Color pauseColor;
    public InputActionAsset menuInputAction;

    private InputAction menuPressed;
    private Color defaulColor;
    private bool followCam;

    private void Start()
    {
        FollowCam(true);
        defaulColor = camImage.color;
        foreach (Transform t in menu)
        {
            t.gameObject.SetActive(false);
        }
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
        GameManager.instance.SetTargetTimeScale(0);
        GameManager.instance.EnableRays(true);
        GameManager.instance.EnableDirectInteractors(false);
        LocomotionManager.instance.EnableMovement(false);
        LocomotionManager.instance.EnableTurning(false);
        menu.GetChild(0).gameObject.SetActive(true);
        GameManager.instance.IsPaused(true);
        FollowCam(false);
        camImage.color = pauseColor;
        AudioManager.instance.MuteSounds();
    }
    public void CloseMenu()
    {
        foreach (Transform t in menu)
        {
            t.gameObject.SetActive(false);
        }
        GameManager.instance.SetTargetTimeScale(1);
        GameManager.instance.IsPaused(false);
        FollowCam(true);
        if(!GameManager.instance.isUpgrading)
        {
            GameManager.instance.EnableRays(false);
            GameManager.instance.EnableDirectInteractors(true);
        }
        LocomotionManager.instance.EnableMovement(true);
        LocomotionManager.instance.EnableTurning(true);
        camImage.color = defaulColor;
        AudioManager.instance.Unmute();
    }


    private void Update()
    {
        if(followCam)
        {
            menuPivot.rotation = Quaternion.Euler(0f, GameManager.instance.cam.transform.eulerAngles.y, 0f);
        }
    }

    public void FollowCam(bool state)
    {
        followCam = state;
    }
}
