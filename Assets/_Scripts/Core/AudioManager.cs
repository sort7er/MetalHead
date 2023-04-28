using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    public AudioSource[] audioSourcesToMute;

    private float[] currentVolume;

    private void Start()
    {
        currentVolume = new float[audioSourcesToMute.Length];
    }

    public void MuteSounds()
    {
        for (int i = 0; i < audioSourcesToMute.Length; i++)
        {
            audioSourcesToMute[i].Stop();
        }
    }
    public void Unmute()
    {
        for (int i = 0; i < audioSourcesToMute.Length; i++)
        {
            if (audioSourcesToMute[i].gameObject.activeSelf)
            {
                audioSourcesToMute[i].Play();
            }
        }
    }
}
