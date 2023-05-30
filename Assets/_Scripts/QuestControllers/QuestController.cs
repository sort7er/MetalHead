using UnityEngine;

public class QuestController : MonoBehaviour
{
    public bool left;
    public Animator controllerAnim;
    public MeshRenderer[] inputs;

    public Material defaultMaterial, highlightMaterial;

    private Animator displayAnim;
    private AudioSource controllerSource;
    private bool includeSound;

    private void Awake()
    {
        displayAnim = GetComponent<Animator>();
        controllerSource = GetComponent<AudioSource>();
    }

    public void Primary(bool sound)
    {
        Commons(sound);
        controllerAnim.Play("Primary");
        inputs[0].material = highlightMaterial;
    }
    public void Secondary(bool sound)
    {
        Commons(sound);
        controllerAnim.Play("Secondary");
        inputs[1].material = highlightMaterial;
    }
    public void Menu(bool sound)
    {
        Commons(sound);
        controllerAnim.Play("Menu");
        inputs[2].material = highlightMaterial;
    }
    public void Trigger(bool sound)
    {
        Commons(sound);
        controllerAnim.Play("Trigger");
        inputs[3].material = highlightMaterial;
    }
    public void Grip(bool sound)
    {
        Commons(sound);
        controllerAnim.Play("Grip");
        inputs[4].material = highlightMaterial;
    }
    public void Joystick(int direction, bool sound)
    {
        Commons(sound);
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

    private void Commons(bool sound)
    {
        Nothing();
        //if (sound && !controllerSource.isPlaying)
        //{
        //    //controllerSource.Play();
        //}
        InvokeRepeating(nameof(SendPulse), 0, controllerSource.clip.length);
    }
    private void SendPulse()
    {
        if (left)
        {
            GameManager.instance.leftHand.SendPulse(0.25f, 0.25f);
        }
        else
        {
            GameManager.instance.rightHand.SendPulse(0.25f, 0.25f);
        }
    }

    public void Nothing()
    {
        controllerAnim.Play("Nothing");
        //controllerSource.Stop();
        CancelInvoke(nameof(SendPulse));

        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].material = defaultMaterial;
        }
    }
    public void QuestActive(bool state)
    {
        if(state)
        {
            CancelInvoke(nameof(Hide));
            controllerAnim.gameObject.SetActive(true);
        }
        else
        {
            Nothing();
            Invoke(nameof(Hide), 0.3f);
        }

        if(displayAnim != null)
        {
            displayAnim.SetBool("Display", state);
        }
    }

    private void Hide()
    {
        controllerAnim.gameObject.SetActive(false);
    }
}
