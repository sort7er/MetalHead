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
            if (other.transform.parent != transform.parent && !cannotCollide)
            {
                currentCollidingEnemy = other.GetComponentInParent<RunningEnemy>();
                AIManager.instance.Avoidance(runningEnemy, currentCollidingEnemy);
                cannotCollide = true;
                Debug.Log("1");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponentInParent<RunningEnemy>() == currentCollidingEnemy && cannotCollide)
        {
            AIManager.instance.AvoidanceDone(runningEnemy, currentCollidingEnemy);
            currentCollidingEnemy = null;
            cannotCollide = false;
            Debug.Log("2");
        }
    }

}
