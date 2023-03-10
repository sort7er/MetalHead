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
            if (collision.transform.GetComponent<RunningEnemy>() != null)
            {
                collision.transform.GetComponent<RunningEnemy>().EnemyAlert(collision.contacts[0].point);
            }
        }
        thrown = false;
    }
}
