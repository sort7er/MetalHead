using UnityEngine;
using UnityEngine.UI;

public class Icons : MonoBehaviour
{
    public Image circle, noAmmo;

    private Color startColor;
    private Animator iconAnim;
    //private Transform currentTarget;

    private void Start()
    {
        iconAnim= GetComponent<Animator>();
        startColor = noAmmo.color;
    }

    private void Update()
    {
        transform.rotation = GameManager.instance.cam.transform.rotation;
        //if (currentTarget != null)
        //{
        //    transform.position = currentTarget.position;
        //}
    }

    public void ShowIcon(int iconIndex, Vector3 targetPos)
    {
        CancelInvoke(nameof(HideIcon));
        transform.position= targetPos;
        iconAnim.SetBool("Show", true);
        if(iconIndex == 0)
        {
            circle.gameObject.SetActive(true);
            noAmmo.gameObject.SetActive(false);
        }
        else
        {
            circle.gameObject.SetActive(false);
            noAmmo.gameObject.SetActive(true);
        }

    }
    public void IconDone()
    {
        iconAnim.SetBool("Show", false);
        Invoke(nameof(HideIcon), 0.25f);

    }
    private void HideIcon()
    {
        circle.gameObject.SetActive(false);
        noAmmo.gameObject.SetActive(false);
    }

    public void ChangeColor(Color newColor)
    {
        CancelInvoke(nameof(ChangeColorBack));
        noAmmo.color = newColor;
        Invoke(nameof(ChangeColorBack), 0.1f);
    }

    private void ChangeColorBack()
    {
        noAmmo.color = startColor;
    }
}
