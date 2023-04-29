using TMPro;
using UnityEngine;

public class ReloadGun : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public string[] textEachStep;
    public GameObject[] reloadDisplays;

    private int currentDisplay;
    private TypeWriterText typeWriterText;
    private void Awake()
    {
        typeWriterText = infoText.GetComponent<TypeWriterText>();
    }
    public void Display()
    {
        typeWriterText.StopTyping();
        infoText.text = "";
        for (int i = 0; i < reloadDisplays.Length; i++)
        {
            reloadDisplays[i].SetActive(false);
        }
        
        if(currentDisplay < reloadDisplays.Length)
        {
            reloadDisplays[currentDisplay].SetActive(true);
            infoText.text = textEachStep[currentDisplay];
        }

        Invoke(nameof(Delay), 0.01f);

        currentDisplay++;

    }
    private void Delay()
    {
        if (typeWriterText.gameObject.activeSelf && typeWriterText.transform.parent.gameObject.activeSelf)
        {
            typeWriterText.StartTyping();
        }
    }

}
