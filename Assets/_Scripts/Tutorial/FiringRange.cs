using TMPro;
using UnityEngine;

public class FiringRange : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Target[] targetsToHit;
    public Animator[] targetsAnim;

    private TypeWriterText typeWriterText;
    private int score;

    private bool cannotReset;

    private void Start()
    {
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
        for (int i = 0; i < targetsAnim.Length; i++)
        {
            targetsAnim[i].SetBool("Lift", true);
            targetsToHit[i].gameObject.SetActive(true);
        }
        Invoke(nameof(LiftUpDone), 2);
    }
    private void LiftUpDone()
    {
        cannotReset = false;
    }
}
