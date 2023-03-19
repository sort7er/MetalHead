using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering.PostProcessing;

public class RigConstraints : MonoBehaviour
{
    public RunningEnemy runningEnemy;
    public Transform targetTransform;
    public Transform headTrans;
    public float tooCloseDistance, angle;

    private Rig rig;
    private bool rigEnabled, tooClose;


    private void Start()
    {
        rig = GetComponent<Rig>();
        rig.weight = 0;
    }

    private void Update()
    {
        if (rigEnabled && !tooClose)
        {
            if (rig.weight < 1)
            {
                rig.weight += Time.deltaTime;
            }
            else
            {
                rig.weight = 1;
            }
        }
        else
        {
            if (rig.weight > 0)
            {
                rig.weight -= Time.deltaTime;
            }
            else
            {
                rig.weight = 0;
            }
        }

        if(Vector3.Angle(targetTransform.position - headTrans.position, runningEnemy.transform.forward) <= angle)
        {
            tooClose = false;
        }
        else
        {
            tooClose = true;
        }
    }

    public void SetRig(bool state)
    {
        rigEnabled = state;
    }
    public void SetTarget(Vector3 newPos)
    {
        targetTransform.position = newPos;
    }
}
