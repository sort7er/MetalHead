using UnityEngine;

public class EnemyRoom : MonoBehaviour
{

    public TutorialManager tutorialManager;

    public void Entered()
    {
        tutorialManager.SetShotgun(true);
    }
}
