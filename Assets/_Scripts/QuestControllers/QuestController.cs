using UnityEngine;

public class QuestController : MonoBehaviour
{
    public bool left;
    public MeshRenderer[] inputs;

    public Material defaultMaterial, highlightMaterial;

    private Animator controllerAnim;
    private AudioSource controllerSource;

    private void Start()
    {
        controllerAnim = GetComponent<Animator>();
        controllerSource = GetComponent<AudioSource>();
    }

    public void Primary()
    {
        Commons();
        controllerAnim.Play("Primary");
        inputs[0].material = highlightMaterial;
    }
    public void Secondary()
    {
        Commons();
        controllerAnim.Play("Secondary");
        inputs[1].material = highlightMaterial;
    }
    public void Menu()
    {
        Commons();
        controllerAnim.Play("Menu");
        inputs[2].material = highlightMaterial;
    }
    public void Trigger()
    {
        Commons();
        controllerAnim.Play("Trigger");
        inputs[3].material = highlightMaterial;
    }
    public void Grip()
    {
        Commons();
        controllerAnim.Play("Grip");
        inputs[4].material = highlightMaterial;
    }
    public void Joystick(int direction)
    {
        Commons();
        if (direction == 1)
        {
            controllerAnim.Play("JoystickDown");
        }
        else if (direction == 2)
        {
            controllerAnim.Play("JoystickSide");
        }
        else
        {
            controllerAnim.Play("JoystickUp");
        }


        inputs[5].material = highlightMaterial;
    }

    private void Commons()
    {
        Nothing();
        controllerSource.Play();
        InvokeRepeating(nameof(SendPulse), 0, controllerSource.clip.length);
    }
    private void SendPulse()
    {
        if (left)
        {
            GameManager.instance.leftHand.SendPulse(0.125f, 0.125f);
        }
        else
        {
            GameManager.instance.rightHand.SendPulse(0.125f, 0.125f);
        }
    }

    public void Nothing()
    {
        controllerAnim.Play("Nothing");
        controllerSource.Stop();

        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].material = defaultMaterial;
        }
    }


}
