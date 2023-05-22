using UnityEngine;

public class CloseToEnemyCheck : MonoBehaviour
{
    private RunningEnemy runningEnemy;
    private RunningEnemy currentCollidingEnemy;

    private bool cannotCollide;

    private void Start()
    {
        runningEnemy = GetComponentInParent<RunningEnemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && other.GetComponent<SoundForEnemy>() != null)
        {
            if (other.transform.parent != transform.parent && !cannotCollide && other.GetComponentInParent<RunningEnemy>() != null)
            {
                currentCollidingEnemy = other.GetComponentInParent<RunningEnemy>();
                AIManager.instance.Avoidance(runningEnemy, currentCollidingEnemy);
                cannotCollide = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(currentCollidingEnemy != null && other.GetComponentInParent<RunningEnemy>() == currentCollidingEnemy && cannotCollide)
        {
            AIManager.instance.AvoidanceDone(runningEnemy, currentCollidingEnemy);
            currentCollidingEnemy = null;
            cannotCollide = false;
        }
    }

}
