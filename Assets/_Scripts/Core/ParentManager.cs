using UnityEngine;
using UnityEngine.PlayerLoop;

public class ParentManager : MonoBehaviour
{
    public static ParentManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Transform effects, bullets, mags, pickups, enemies;
}
