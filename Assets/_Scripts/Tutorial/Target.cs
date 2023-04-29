using UnityEngine;

public class Target : MonoBehaviour
{
    public FiringRange firingRange;
    public Transform center;

    private float radius, distance, maxDistance;

    private void Start()
    {
        radius = transform.localScale.x * 0.22f;
        maxDistance = Vector3.Distance(transform.position, new Vector3(transform.position.x + radius, transform.position.y, transform.position.z));
    }

    public void Hit(Vector3 hitPos)
    {
        distance = Vector3.Distance(hitPos, center.position);
        int score = Mathf.CeilToInt(Mathf.Abs(distance /maxDistance * 100 - 10));
        firingRange.AddScore(score);
        gameObject.SetActive(false);
        EffectManager.instance.SpawnMessage(score.ToString());
    }
}
