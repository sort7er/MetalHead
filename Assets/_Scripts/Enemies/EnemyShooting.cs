using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform bombMuzzle;
    public GameObject bombPrefab;

    private float multiplier;

    public void FireBomb()
    {
        Debug.Log("Fire");
        GameObject bomb = Instantiate(bombPrefab, bombMuzzle.position, bombMuzzle.rotation);
        bomb.transform.parent = ParentManager.instance.bullets;

        float distance = Vector3.Distance(transform.position, GameManager.instance.XROrigin.transform.position);
        float heightDifference = GameManager.instance.XROrigin.transform.position.y - transform.position.y;

        Debug.Log(heightDifference);

        if (heightDifference < 0.1f)
        {
            multiplier = 1;
        }
        else
        {
            multiplier = 1 + (heightDifference * 0.005f);
        }

        bomb.GetComponent<Rigidbody>().AddForce(bombMuzzle.forward * distance * 100 * multiplier, ForceMode.Impulse);

    }
}
