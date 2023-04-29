using TMPro;
using UnityEngine;

public class InfoMessage : MonoBehaviour
{
    [Header("Message")]
    public string messageToDisplay;
    public string messageToDisplay2;

    [Header("Animation")]
    public string animationStateName;

    [Header("References")]
    public Animator animatorToPlay;
    public TextMeshProUGUI infoText;

    private TypeWriterText typeWriterText;

    private void Awake()
    {
        typeWriterText = infoText.GetComponent<TypeWriterText>();
    }

    private void OnEnable()
    {
        typeWriterText.StopTyping();
        infoText.text = messageToDisplay;
        Invoke(nameof(Delay), 0.01f);
        if (messageToDisplay2 != "")
        {
            Invoke(nameof(SecondMessage), 7);
        }
        if(animatorToPlay != null)
        {
            animatorToPlay.Play(animationStateName);
        }
    }
    private void OnDisable()
    {
        typeWriterText.StopTyping();
    }
    private void Delay()
    {
        if (typeWriterText.gameObject.activeSelf && typeWriterText.transform.parent.gameObject.activeSelf)
        {
            typeWriterText.StartTyping();
        }
    }
    private void SecondMessage()
    {
        infoText.text = messageToDisplay2;
        Invoke(nameof(Delay), 0.01f);
        if (messageToDisplay != "")
        {
            Invoke(nameof(FirstMessage), 10);
        }
    }
    private void FirstMessage()
    {
        infoText.text = messageToDisplay;
        Invoke(nameof(Delay), 0.01f);
        if (messageToDisplay != "")
        {
            Invoke(nameof(SecondMessage), 7);
        }
    }
}
