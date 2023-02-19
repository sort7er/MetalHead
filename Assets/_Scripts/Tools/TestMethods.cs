using UnityEngine.Events;
using UnityEngine;

public class TestMethods : MonoBehaviour
{
    public UnityEvent onPressP, onReleaseP, onPressR;
    public GameObject magazinePrefab;
    public Transform spawn;

    private GameObject magazine;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            onPressP.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            onReleaseP.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            onPressR.Invoke();
        }
    }

    public void One()
    {
        Debug.Log("1");
    }
    public void Two()
    {
        Debug.Log("2");
    }
    public void Three()
    {
        Debug.Log("3");
    }
    public void Four()
    {
        Debug.Log("4");
    }
    public void InstatntiateMagazine()
    {
        magazine = Instantiate(magazinePrefab, spawn.position, Quaternion.identity);
        magazine.GetComponent<Rigidbody>().isKinematic= true;
        magazine.GetComponent<Rigidbody>().useGravity=false;
    }
    //public void DestroyMagazine()
    //{
    //    Destroy(magazine);
    //}
}
