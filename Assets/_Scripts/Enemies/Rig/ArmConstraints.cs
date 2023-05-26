using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArmConstraints : MonoBehaviour
{
    public Transform target;
    public MultiAimConstraint shoulder;
    public MultiAimConstraint tricep;
    public MultiAimConstraint arm;

    private float targetWeight, currentWeight;
    private bool canAim;


    void Update()
    {
        arm.weight = tricep.weight = shoulder.weight = currentWeight;


        if(currentWeight < targetWeight - 0.1f)
        {
            currentWeight += Time.deltaTime * 4f;
        }
        else if(currentWeight > targetWeight + 0.1f)
        {
            currentWeight -= Time.deltaTime * 4f;
        }
        else
        {
            currentWeight = targetWeight;
        }


        if (Vector3.Angle(target.position - transform.position, transform.forward) > 80/* && target.localPosition.x < 0 && target.localPosition.z > 0*/)
        {
            if (canAim)
            {
                canAim= false;
            }
        }
        //else if (target.localPosition.x < 0 && target.localPosition.z < 0)
        //{
        //    if (canAim)
        //    {
        //        canAim = false;
        //    }
        //}
        else
        {
            if (!canAim)
            {
                canAim = true;
            }
        }

        if (canAim)
        {
            targetWeight = 1;
        }
        else
        {
            targetWeight = 0;
        }


    }

    public bool CanAim()
    {
        return canAim;
    }
}
