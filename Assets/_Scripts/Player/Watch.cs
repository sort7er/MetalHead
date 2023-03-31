using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Watch : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    public Image sliderFill;
    public Gradient healthColors;

    public void SetNewHealth(int health, int maxHealth)
    {
        healthText.text = health.ToString();

        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        sliderFill.color = healthColors.Evaluate(healthSlider.normalizedValue);
    }

    public void UpdateHealth(int health)
    {
        healthText.text = health.ToString();
        healthSlider.value = health;
        sliderFill.color = healthColors.Evaluate(healthSlider.normalizedValue);
    }
}
