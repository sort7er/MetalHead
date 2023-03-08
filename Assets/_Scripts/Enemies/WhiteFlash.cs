using UnityEngine;

public class WhiteFlash : MonoBehaviour
{
    public float flashDuration;
    public MeshRenderer[] meshesToFlash;
    
    private Color[] originalColors;

    private void Start()
    {
        originalColors = new Color[meshesToFlash.Length];
        for(int i = 0; i < originalColors.Length; i++)
        {
            originalColors[i] = meshesToFlash[i].material.color;
        }
    }

    public void Flash()
    {
        for (int i = 0; i < meshesToFlash.Length; i++)
        {
            meshesToFlash[i].material.color = Color.white;
        }
        Invoke("ResetColors", flashDuration);
    }
    private void ResetColors()
    {
        for (int i = 0; i < meshesToFlash.Length; i++)
        {
            meshesToFlash[i].material.color = originalColors[i];
        }
    }
}
