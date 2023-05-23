using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform bombMuzzle;
    public GameObject bombPrefab;

    public void FireBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, bombMuzzle.position, bombMuzzle.rotation);
        bomb.transform.parent = ParentManager.instance.bullets;

        float distance = Vector3.Distance(transform.position, GameManager.instance.XROrigin.transform.position);

        bomb.GetComponent<Rigidbody>().AddForce(bombMuzzle.forward * distance, ForceMode.Impulse);

    }
}
