using UnityEditor;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startHealth;
    public float startPosture;
    public float postureRegenerationSpeed;
    public float timeBeforeHide;

    private RunningEnemy runningEnemy;
    private Vector3 damageDir;
    private Rigidbody rb;
    private bool isDead;
    private int health, currentBodyPart;
    private float posture, hideTimer;

    private void Start()
    {
        if(GetComponent<RunningEnemy>() != null)
        {
            runningEnemy = GetComponent<RunningEnemy>();
        }
        posture = startPosture;
        health = startHealth;
    }
    private void Update()
    {
        if (posture < startPosture)
        {
            posture += Time.deltaTime * postureRegenerationSpeed;
        }
        else
        {
            posture = startPosture;
        }

        if(hideTimer > 0)
        {
            hideTimer -= Time.deltaTime;
            if(hideTimer >= timeBeforeHide)
            {
                runningEnemy.Hide();
                hideTimer = 0;
            }
        }
        else
        {
            hideTimer = 0;
        }
    }

    public void TakeDamage(int damage, float stun, Rigidbody rb1, Vector3 damageDir1, int bodyPart)
    {
        if (!isDead)
        {
            rb = rb1;
            damageDir = damageDir1;
            
            TakeStun(stun, bodyPart);
            health -= damage;
            if (health < 0)
            {
                Die();
            }
            hideTimer += startHealth * 0.01f - health * 0.01f + 1;
        }
    }
    public void TakeStun(float stun, int bodyPart)
    {
        posture -= stun;
        currentBodyPart = bodyPart;
        if (posture < 0)
        {
            Stun();
        }
    }

    private void Die()
    {
        isDead = true;
        runningEnemy.Die();
        runningEnemy.AddForce(rb, damageDir);
        EffectManager.instance.SpawnPickups(transform, Random.Range(3, 8));

    }
    private void Stun()
    {
        posture = startPosture;
        runningEnemy.Stun(currentBodyPart);
    }
}
