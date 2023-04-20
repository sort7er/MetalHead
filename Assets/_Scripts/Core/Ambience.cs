using UnityEngine;

public class Ambience : MonoBehaviour
{
    private GameObject[] ambience;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ambience = GameObject.FindGameObjectsWithTag("Ambience");
        if (ambience.Length > 1)
        {
            Destroy(ambience[1]);
        }
    }
}
