using TMPro;
using UnityEngine;

public class PopUpMessage : MonoBehaviour
{
    public TextMeshProUGUI message;

    public void SetMessage(string text)
    {
        message.text = text;
    }
}
