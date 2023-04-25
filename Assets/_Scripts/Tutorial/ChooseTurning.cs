using TMPro;
using UnityEngine;

public class ChooseTurning : MonoBehaviour
{
    public TextMeshProUGUI turnText;



    private void OnEnable()
    {
        if (LocomotionManager.instance != null && LocomotionManager.instance.currentTurnType == 1)
        {
            Debug.Log("1");
            turnText.text = "Continuous turning";
        }
        else
        {
            Debug.Log("2");
            turnText.text = "Snap turning";
        }
    }


    public void SwitchTurning()
    {
        if (LocomotionManager.instance.currentTurnType == 0)
        {
            turnText.text = "Continuous turning";
        }
        else
        {
            turnText.text = "Snap turning";
        }
        LocomotionManager.instance.SwitchTurning();
    }

}
