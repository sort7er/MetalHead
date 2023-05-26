using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigConstraints : MonoBehaviour
{
    public RunningEnemy runningEnemy;
    public Transform targetTransform;
    public Transform headTrans;
    public float tooCloseDistance, angle;

    private Rig rig;
    private Vector3 newPos;
    private bool rigEnabled, tooClose, newTarget;


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
                rig.weight += Time.deltaTime * 4;
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
                rig.weight -= Time.deltaTime * 4;
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
        if (newTarget)
        {
            targetTransform.position = Vector3.Lerp(targetTransform.position, newPos, Time.deltaTime * 10);
            if((newPos - targetTransform.position).magnitude <= 0.2f)
            {
                newTarget = false;
            }
        }

    }

    public void SetRig(bool state)
    {
        rigEnabled = state;
    }
    public void SetTarget(Vector3 lookAtPos)
    {
        newTarget = true;
        newPos = lookAtPos;
    }
}
