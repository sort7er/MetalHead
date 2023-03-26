using TMPro;
using UnityEngine;

public class Watch : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    public void SetNewHealth(int health)
    {
        healthText.text = health.ToString();
    }
}
