using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Watch : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    public Image sliderFill;
    public Gradient healthColors;
    public GameObject healthRing;
    public GameObject healthRing2;
    public GameObject healthRing3;

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
    public void HealthRing(int currentLevel)
    {
        if(currentLevel == 0)
        {
            healthRing.SetActive(true);
        }
        else if(currentLevel == 1)
        {
            healthRing2.SetActive(true);
        }
        else if (currentLevel == 2)
        {
            healthRing3.SetActive(true);
        }
    }
}
