using TMPro;
using UnityEngine;

public class FiringRange : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject ammoPrefab;
    public Transform ammoSpawn;

    public Animator[] targetsAnim;

    private TypeWriterText typeWriterText;
    private Target[] targetsToHit;
    private GameObject currentAmmo;
    private int score;
    private bool cannotReset;


    private void Start()
    {
        targetsToHit = new Target[targetsAnim.Length];
        for (int i = 0; i < targetsAnim.Length; i++)
        {
            if(targetsAnim[i].GetComponentInChildren<Target>() != null)
            {
                targetsToHit[i] = targetsAnim[i].GetComponentInChildren<Target>();
            }
        }

        typeWriterText = scoreText.GetComponent<TypeWriterText>();
        ResetScore();
    }

    public void AddScore(int scoreToAdd)
    {
        typeWriterText.StopTyping();
        score += scoreToAdd;
        scoreText.text = score.ToString();
        typeWriterText.StartTyping();
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

            for(int i = 0; i < targetsAnim.Length; i++)
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
        Invoke(nameof(LiftUpDone), 2f);
    }
    private void LiftUpDone()
    {
        cannotReset = false;
    }
}
