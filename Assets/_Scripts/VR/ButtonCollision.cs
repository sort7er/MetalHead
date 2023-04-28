using UnityEngine;

public class ButtonCollision : MonoBehaviour
{

    public PhysicsButton physicsButton;

    private int numberOfCollisions;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 16 || collision.gameObject.layer == 15 || collision.gameObject.layer == 8)
        {
            numberOfCollisions++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 16 || collision.gameObject.layer == 15 || collision.gameObject.layer == 8)
        {
            numberOfCollisions--;
             if(numberOfCollisions <= 0)
            {
                physicsButton.Unfreeze();
            }
        }
    }
}
