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
        Destroy(ambience[1]);
    }
}
