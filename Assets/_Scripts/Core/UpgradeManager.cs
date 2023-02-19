using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    private void Awake()
    {
        instance= this;
    }

    [Header("CZ50")]
    public int magSize;
    


    public void SetMagSize(int size)
    {
        magSize = size;
    }
    public int GetMagSize() { return magSize; }
}
