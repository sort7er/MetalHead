using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    public AudioClip hover, click, clickBack;

    private AudioSource menuSource;

    private void Start()
    {
        menuSource = GetComponent<AudioSource>();
    }

    public void Hover()
    {
        menuSource.PlayOneShot(hover);
    }
    public void Click()
    {
        menuSource.PlayOneShot(click);
    }
    public void ClickBack()
    {
        menuSource.PlayOneShot(clickBack);
    }
}
