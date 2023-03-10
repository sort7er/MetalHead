using UnityEngine;

public class MagnetAnimation : MonoBehaviour
{
    private Animator magnetAnim;
    private MagnetSounds magnetSounds;
    // Start is called before the first frame update
    void Start()
    {
        magnetAnim = GetComponent<Animator>();
        magnetSounds = GetComponent<MagnetSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GrabMagnetRelay()
    {
        Invoke("GrabMagnetAnim", 0.5f);
    }
    private void GrabMagnetAnim()
    {
        magnetAnim.SetBool("Out", true);
        magnetSounds.MagnetActivate(0);
    }
    public void ReleaseMagnetAnim()
    {
        CancelInvoke();
        magnetAnim.SetBool("Out", false);
    }
}
