using UnityEngine;

public class VoiceLines : MonoBehaviour
{

    public AudioClip[] idleToSus;
    public AudioClip[] susToIdle;
    public AudioClip[] idleToRun;
    public AudioClip[] runToSearching;
    public AudioClip[] searchingToRun;
    public AudioClip[] hiding;
    public AudioClip[] attacking;
    public AudioClip[] dying;

    private AudioSource voiceLinesSource;

    void Start()
    {
        voiceLinesSource = GetComponent<AudioSource>();
    }

    
}
