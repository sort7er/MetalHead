using TMPro;
using UnityEngine;

public class FiringRange : MonoBehaviour
{
    public float startTime;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetsLeftText;
    public GameObject ammoPrefab;
    public Transform ammoSpawn;

    public Animator[] targetsAnim;

    private TypeWriterText typeWriterText;
    private TypeWriterText typeWriterTextTargetsLeft;
    private Target[] targetsToHit;
    private GameObject currentAmmo;
    private int score;
    private bool cannotReset, started;
    private float timer;
    private int numberOfTargets, targetsLeft;


    private void Start()
    {
        targetsToHit = new Target[targetsAnim.Length];
        for (int i = 0; i < targetsAnim.Length; i++)
        {
            if(targetsAnim[i].GetComponentInChildren<Target>() != null)
            {
                targetsToHit[i] = targetsAnim[i].GetComponentInChildren<Target>();
                numberOfTargets++;
            }
        }
    
        typeWriterText = scoreText.GetComponent<TypeWriterText>();
        typeWriterTextTargetsLeft = targetsLeftText.GetComponent<TypeWriterText>();
        ResetScore();
    }
    private void Update()
    {
        if (started)
        {
            timer -= Time.deltaTime; 
            if(timer <= 0)
            {
                timer = 0;
                Stop();
            }
            else if(targetsLeft >= numberOfTargets)
            {
                Stop();
            }

            timerText.text = "Time left: " + timer.ToString("F2");
        }
    }

    public void AddScore(int scoreToAdd)
    {
        typeWriterText.StopTyping();
        score += scoreToAdd;
        scoreText.text = score.ToString();
        typeWriterText.StartTyping();

        typeWriterTextTargetsLeft.StopTyping();
        targetsLeft++;
        targetsLeftText.text = targetsLeft.ToString() + " / " + numberOfTargets.ToString();
        typeWriterTextTargetsLeft.StartTyping();

        if(!started)
        {
            StartCountdown();
        }
    }
    public void Stop()
    {
        started = false;
        typeWriterText.StopTyping();
        scoreText.text = score.ToString();
        typeWriterText.StartTyping();

        for (int i = 0; i < targetsAnim.Length; i++)
        {
            targetsAnim[i].SetBool("Lift", false);
        }
    }
    public void ResetScore()
    {
        if (!cannotReset)
        {
            typeWriterText.StopTyping();
            score = 0;
            scoreText.text = score.ToString();
            typeWriterText.StartTyping();
            cannotReset = true;

            targetsLeft = 0;
            typeWriterTextTargetsLeft.StopTyping();
            targetsLeftText.text = "";

            timerText.text = "";

            for (int i = 0; i < targetsAnim.Length; i++)
            {
                targetsAnim[i].SetBool("Lift", false);
            }
            Invoke(nameof(LiftUp), 2);
        }
    }
    private void LiftUp()
    {
        if(currentAmmo == null)
        {
            currentAmmo = Instantiate(ammoPrefab, ammoSpawn.position, Quaternion.identity);
        }
        else if (currentAmmo != null && Vector3.Distance(ammoSpawn.position, currentAmmo.transform.position) > 4f)
        {
            Destroy(currentAmmo);
            currentAmmo = Instantiate(ammoPrefab, ammoSpawn.position, Quaternion.identity);
        }
        for (int i = 0; i < targetsAnim.Length; i++)
        {
            targetsAnim[i].SetBool("Lift", true);
            if (targetsToHit[i] != null)
            {
                targetsToHit[i].gameObject.SetActive(true);
            }
        }
        started = false;
        Invoke(nameof(LiftUpDone), 2f);
    }
    private void LiftUpDone()
    {
        cannotReset = false;
    }

    public void StartCountdown()
    {
        timer = startTime;
        started = true;
    }
}
