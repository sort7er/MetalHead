using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public Transform menuPivot;
    public InputActionAsset menuInputAction;

    private InputAction menuPressed;
    private bool paused;
    private bool followCam = true;

    private void Start()
    {
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
        if(!paused)
        {
            OpenMenu();
        }
        else
        {
            CloseMenu();
        }
    }
    public void OpenMenu()
    {
        GameManager.instance.EnableRays(true);
        LocomotionManager.instance.EnableMovement(false);
        LocomotionManager.instance.EnableTurning(false);
        menu.SetActive(true);
        paused = true;
        followCam= false;
    }
    public void CloseMenu()
    {
        menu.SetActive(false);
        paused = false;
        followCam = true;
        GameManager.instance.EnableRays(false);
        LocomotionManager.instance.EnableMovement(true);
        LocomotionManager.instance.EnableTurning(true);
    }


    private void Update()
    {
        if(followCam)
        {
            menuPivot.rotation = Quaternion.Euler(0f, GameManager.instance.cam.transform.eulerAngles.y, 0f);
        }
    }
}
