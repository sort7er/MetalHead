using TMPro;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public EnemyHealth[] enemiesInArea;
    public TextMeshProUGUI enemiesKilledText;

    public Material redGlow;
    public Material greenGlow;
    public MeshRenderer[] reds;
    public MeshRenderer[] greens;


    private AudioSource gateSource;
    private Material startMaterial;

    private Animator gateAnim;

    private bool isOpen;
    private int numberOfDeadEnemies;


    private void Start()
    {
        startMaterial = greens[0].material;

        gateSource = GetComponent<AudioSource>();
        gateAnim = GetComponent<Animator>();

        if (enemiesInArea.Length > 0)
        {
            enemiesKilledText.text = "0 / " + enemiesInArea.Length;
        }
        else
        {
            enemiesKilledText.text = "0 / 0";
        }

        for(int i = 0; i < reds.Length; i++)
        {
            reds[i].material = redGlow;
            greens[i].material = startMaterial;
        }

    }



    public void CheckIfDead()
    {
        if(enemiesInArea.Length > 0)
        {
            numberOfDeadEnemies = 0;
            for (int i = 0; i < enemiesInArea.Length; i++)
            {
                if (enemiesInArea[i].IsDead())
                {
                    numberOfDeadEnemies++;
                }

            }

            enemiesKilledText.text = numberOfDeadEnemies.ToString() + " / " + enemiesInArea.Length;

            if (numberOfDeadEnemies >= enemiesInArea.Length)
            {
                OpenGate();
            }
        }
    }


    public void OpenGate()
    {
        if(!isOpen)
        {
            isOpen = true;
            gateAnim.SetTrigger("Open");
            gateSource.Play();
            for (int i = 0; i < reds.Length; i++)
            {
                reds[i].material = startMaterial;
                greens[i].material = greenGlow;
            }
        }
    }
}
