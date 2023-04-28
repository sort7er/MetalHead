using TMPro;
using UnityEngine;

public class InfoMessage : MonoBehaviour
{
    [Header("Message")]
    public string messageToDisplay;

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
        infoText.text = messageToDisplay;
        Invoke(nameof(Delay), 0.01f);
        animatorToPlay.Play(animationStateName);
    }
    private void OnDisable()
    {
        typeWriterText.StopTyping();
    }
    private void Delay()
    {
        typeWriterText.StartTyping();
    }
}
