using UnityEngine;

public class WhiteFlash : MonoBehaviour
{
    public float flashDuration, flashInInterpolation, flashOutInterpolation;
    public MeshRenderer[] meshesToFlash;
    
    private Color[] originalColors;
    private bool flash;
    private float flashSpeed;

    private void Start()
    {
        originalColors = new Color[meshesToFlash.Length];
        for(int i = 0; i < originalColors.Length; i++)
        {
            originalColors[i] = meshesToFlash[i].material.color;
        }
    }

    private void Update()
    {
        if (flash)
        {
            for (int i = 0; i < meshesToFlash.Length; i++)
            {
                meshesToFlash[i].material.color = Color.Lerp(meshesToFlash[i].material.color, Color.white, Time.deltaTime * flashSpeed);
            }
        }
        else
        {
            for (int i = 0; i < meshesToFlash.Length; i++)
            {
                meshesToFlash[i].material.color = Color.Lerp(meshesToFlash[i].material.color, originalColors[i], Time.deltaTime * flashSpeed);
            }
        }

    }

    public void Flash()
    {
        flash = true;
        flashSpeed = flashInInterpolation;
        Invoke("ResetColors", flashDuration);
    }
    private void ResetColors()
    {
        flash = false;
        flashSpeed = flashOutInterpolation;
    }
}
