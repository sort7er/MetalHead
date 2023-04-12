using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HitConstraints : MonoBehaviour
{
    public float hitSpeed, retractSpeed;

    public MultiRotationConstraint neckConstraint;
    public MultiRotationConstraint stomachConstraint;
    public MultiRotationConstraint leftShoulderConstraint;
    public MultiRotationConstraint rightShoulderConstraint;

    public Transform neckTarget;
    public Transform stomachTarget;
    public Transform leftShoulderTarget;
    public Transform rightShoulderTarget;

    private bool head, headDone;
    private bool stomach, stomachDone;
    private bool leftShoulder, leftShoulderDone;
    private bool rightShoulder, rightShoulderDone;

    public void Hit(bool forward, int bodyPart)
    {
        if(forward)
        {
            if(bodyPart== 1)
            {
                neckTarget.localRotation= Quaternion.Euler(15f, 0, 0);
                head = true;
                headDone = false;
            }
            else if (bodyPart == 6)
            {
                stomachTarget.localRotation = Quaternion.Euler(20f, 0, 0);
                stomach = true;
                stomachDone = false;
            }
            else if (bodyPart == 2)
            {
                leftShoulderTarget.localRotation = Quaternion.Euler(-25f, 0, 0);
                leftShoulder = true;
                leftShoulderDone = false;
            }
            else if (bodyPart == 3)
            {
                rightShoulderTarget.localRotation = Quaternion.Euler(-25f, 0, 0);
                rightShoulder = true;
                rightShoulderDone = false;
            }
        }
        else
        {
            if (bodyPart == 1)
            {
                neckTarget.localRotation = Quaternion.Euler(-15f, 0, 0);
                head = true;
                headDone = false;
            }
            else if (bodyPart == 6)
            {
                stomachTarget.localRotation = Quaternion.Euler(-20f, 0, 0);
                stomach = true;
                stomachDone = false;
            }
            else if (bodyPart == 2)
            {
                leftShoulderTarget.localRotation = Quaternion.Euler(25f, 0, 0);
                leftShoulder = true;
                leftShoulderDone = false;
            }
            else if (bodyPart == 3)
            {
                rightShoulderTarget.localRotation = Quaternion.Euler(25f, 0, 0);
                rightShoulder = true;
                rightShoulderDone = false;
            }
        }

        
    }

    private void Update()
    {
        //Head
        if (head && !headDone)
        {
            if (neckConstraint.weight < 1)
            {
                neckConstraint.weight += Time.deltaTime * hitSpeed;
            }
            else
            {
                head = false;
            }
        }
        else if(!head && !headDone)
        {
            if (neckConstraint.weight > 0)
            {
                neckConstraint.weight -= Time.deltaTime * retractSpeed;
            }
            else
            {
                headDone = true;
                neckConstraint.weight = 0;
            }
        }


        //Stomach
        if (stomach && !stomachDone)
        {
            if (stomachConstraint.weight < 1)
            {
                stomachConstraint.weight += Time.deltaTime * hitSpeed;
            }
            else
            {
                stomach = false;
            }
        }
        else if (!stomach && !stomachDone)
        {
            if (stomachConstraint.weight > 0)
            {
                stomachConstraint.weight -= Time.deltaTime * retractSpeed;
            }
            else
            {
                stomachDone = true;
                stomachConstraint.weight = 0;
            }
        }


        //Left shoulder
        if (leftShoulder && !leftShoulderDone)
        {
            if (leftShoulderConstraint.weight < 1)
            {
                leftShoulderConstraint.weight += Time.deltaTime * hitSpeed;
            }
            else
            {
                leftShoulder = false;
            }
        }
        else if (!leftShoulder && !leftShoulderDone)
        {
            if (leftShoulderConstraint.weight > 0)
            {
                leftShoulderConstraint.weight -= Time.deltaTime * retractSpeed;
            }
            else
            {
                leftShoulderDone = true;
                leftShoulderConstraint.weight = 0;
            }
        }



        //Right shoulder
        if (rightShoulder && !rightShoulderDone)
        {
            if (rightShoulderConstraint.weight < 1)
            {
                rightShoulderConstraint.weight += Time.deltaTime * hitSpeed;
            }
            else
            {
                rightShoulder = false;
            }
        }
        else if (!rightShoulder && !rightShoulderDone)
        {
            if (rightShoulderConstraint.weight > 0)
            {
                rightShoulderConstraint.weight -= Time.deltaTime * retractSpeed;
            }
            else
            {
                rightShoulderDone = true;
                rightShoulderConstraint.weight = 0;
            }
        }

    }

}
