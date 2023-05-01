using UnityEngine;

public class Target : MonoBehaviour
{
   
    public Transform center;
    public Transform edge;


    public GameObject[] targets;

    private int numberOfParts;
    private FiringRange firingRange;
    private float distance, maxDistance;
    private int score;

    private void Start()
    {
        if(FindObjectOfType<FiringRange>() != null)
        {
            firingRange= FindObjectOfType<FiringRange>();
        }
        maxDistance = Vector3.Distance(center.position, edge.position);
    }

    public void Hit(Vector3 hitPos)
    {
        distance = Vector3.Distance(center.position, hitPos);
        score = Mathf.CeilToInt(Mathf.Abs(distance / maxDistance * numberOfParts - numberOfParts));
        firingRange.AddScore(score);
        gameObject.SetActive(false);
        GameObject shatteredTarget = Instantiate(targets[Random.Range(0, targets.Length)], transform.position, transform.rotation, transform);
        shatteredTarget.transform.parent = ParentManager.instance.effects;
        Destroy(shatteredTarget, 3f);
        EffectManager.instance.SpawnMessage(score.ToString(), 0.8f);
    }

    public void SetNumberOfParts(int newDivide)
    {
        numberOfParts = newDivide;
    }
}
