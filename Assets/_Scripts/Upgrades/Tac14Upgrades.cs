using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tac14Upgrades : MonoBehaviour
{
    public UpgradeStation upgradeStation;

    public int[] upgradeCost;

    //public int reloadCost, autoCost, damageCost, magCost, pelletCost, penetrationCost;

    public TextMeshProUGUI[] costText;
    public TextMeshProUGUI[] levelText;
    public GameObject[] outlines;

    //public TextMeshProUGUI reloadCostText, autoCostText, damageCostText, magCostText, pelletCostText, penetrationCostText;
    //public TextMeshProUGUI reloadNumber, recoilNumber, ammoClipNumber, penetrationNumber, laserNumber;
    //public GameObject bulletOutline, recoilOutline, ammoClipOutline, penetrationOutline, laserOutline;
    private Color startTextColor, selectTextColor;

    private int[] levels;
    private int[] startLevels;


    private int reloadLevel, autoLevel, damageLevel, magLevel, pelletLevel, penetrationLevel;
    private int startReloadLevel, startAutoLevel, startDamageLevel, startMagLevel, startPelletLevel, startPenetrationLevel;



    private void Start()
    {
        startTextColor = costText[0].color;
        selectTextColor = outlines[0].GetComponent<Image>().color;
        levels = new int[upgradeCost.Length];
        startLevels = new int[upgradeCost.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            Debug.Log(UpgradeManager.instance.tac14StartLevels[i]);
            levels[i] = UpgradeManager.instance.tac14StartLevels[i];
        }
        SetLevels();
        SetCost();

    }
    public void SetLevels()
    {
        for(int i = 0; i < levels.Length; i++)
        {
            startLevels[i] = levels[i];
            levelText[i].text = levels[i].ToString();
        }
    }
    public void SetCost()
    {

        for(int i = 0; i < costText.Length; i++)
        {
            costText[i].text = upgradeCost[i].ToString();
        }
    }
}
