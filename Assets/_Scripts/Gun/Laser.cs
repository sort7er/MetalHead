using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("References")]
    public GameObject laserHit;
    public Material glassMat, laserMat;
    public MeshRenderer glass;

    [Header("Values")]
    public float offset = 0.1f;

    private LineRenderer laser;
    private bool laserOn;

    private void Start()
    {
        laser = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        if (laserOn)
        {
            laser.enabled = true;
            laser.SetPosition(0, transform.position);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5000, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                laser.SetPosition(1, hit.point);
                laserHit.SetActive(true);
                laserHit.transform.position = hit.point - transform.forward * offset;
                laserHit.transform.rotation = Quaternion.LookRotation(-hit.normal);
            }
            else
            {
                laser.SetPosition(1, transform.forward * 5000);
                laserHit.SetActive(false);
            }
        }
        else
        {
            laser.enabled = false;
            laserHit.SetActive(false);
        }
    }
    public void LaserOn(bool state)
    {
        laserOn = state;
        if (state)
        {
            glass.material = laserMat;
        }
        else
        {
            glass.material = glassMat;
        }
    }
}
