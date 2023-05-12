using UnityEngine;

public class ShotgunSlide : MonoBehaviour
{
    public SoundForGun soundForGun;

    private Vector3 startPos;
    private bool slideValid;

    private void Start()
    {
        startPos = transform.position;
    }

    public void SlideDone()
    {
        if (slideValid)
        {
            soundForGun.SlideBack();
        }
    }
}
