using UnityEngine;

public class Target : MonoBehaviour
{
    public FiringRange firingRange;
    public Transform center;

    private float radius, distance;

    private void Start()
    {
        radius = transform.localScale.x * 0.5f;
    }

    public void Hit(Vector3 hitPos)
    {
        distance = Vector3.Distance(hitPos, center.position) / radius;
        Debug.Log(distance);
        int score = Mathf.CeilToInt(Mathf.Abs(distance * 10 - 10));
        Debug.Log(score);
        firingRange.AddScore(score);
        gameObject.SetActive(false);
    }
}
