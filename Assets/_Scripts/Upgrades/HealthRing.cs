using UnityEngine;

public class HealthRing : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GameManager.instance.XROrigin.GetComponent<PlayerHealth>();
    }

    public void GrabRing()
    {
        playerHealth.UpgradeHealth();
        Destroy(gameObject);
    }
}
