using UnityEngine;

public class CheckIfThrown : MonoBehaviour
{
    private bool thrown;
    public void Released()
    {
        thrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            if (collision.transform.GetComponentInParent<RunningEnemy>() != null)
            {
                collision.transform.GetComponentInParent<RunningEnemy>().EnemyAlert(collision.contacts[0].point);
            }
        }
        thrown = false;
    }
}
