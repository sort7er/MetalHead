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
    private Animator tac14UpgradesAnim;
    private int[] levels;
    private int[] startLevels;


    //private int reloadLevel, autoLevel, damageLevel, magLevel, pelletLevel, penetrationLevel;
    //private int startReloadLevel, startAutoLevel, startDamageLevel, startMagLevel, startPelletLevel, startPenetrationLevel;



    private void Start()
    {
        tac14UpgradesAnim = GetComponent<Animator>();
        startTextColor = costText[0].color;
        selectTextColor = outlines[0].GetComponent<Image>().color;
        levels = new int[upgradeCost.Length];
        startLevels = new int[upgradeCost.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i] = UpgradeManager.instance.tac14StartLevels[i];
        }
        SetLevels();
        SetCost();

    }

    public void ActiveUpgrade(int activeUpgrade)
    {
        tac14UpgradesAnim.SetInteger("Upgrade", activeUpgrade);
    }

    public void Plus(int number)
    {
        if (levels[number] < UpgradeManager.instance.tac14Caps[number])
        {
            if(upgradeStation.currencyOnScreen >= upgradeCost[number])
            {
                upgradeStation.AddingPurchase(upgradeCost[number]);
                levels[number]++;
                levelText[number].text = levels[number].ToString();
                if (!outlines[number].activeSelf)
                {
                    outlines[number].SetActive(true);
                    levelText[number].color = selectTextColor;
                }
                if (levels[number] >= UpgradeManager.instance.tac14Caps[number])
                {
                    levelText[number].text = "Max";
                }
            }
            else
            {
                upgradeStation.NotEnough();
            }
        }
    }
    public void Minus(int number)
    {
        if (levels[number] > startLevels[number])
        {
            upgradeStation.RemovePurchase(upgradeCost[number]);
            levels[number]--;
            levelText[number].text = levels[number].ToString();
            if (levels[number] == startLevels[number])
            {
                outlines[number].SetActive(false);
                levelText[number].color = startTextColor;
            }
        }
    }

    public void Execute()
    {
        UpgradeManager.instance.UpgradeTac14(levels[0], levels[1], levels[2], levels[3], levels[4], levels[5]);
        GameManager.instance.magnet.SetMetalsCollected(upgradeStation.currencyOnScreen);

        for (int i = 0; i < levels.Length; i++)
        {
            outlines[i].SetActive(false);
            levelText[i].color = startTextColor;
        }

        SetLevels();
    }

    public void Abort()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            outlines[i].SetActive(false);
            levelText[i].color = startTextColor;
        }
        ResetLevels();
    }

    private void SetLevels()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            startLevels[i] = levels[i];
            if (levels[i] >= UpgradeManager.instance.tac14Caps[i])
            {
                levelText[i].text = "Max";
            }
            else
            {
                levelText[i].text = levels[i].ToString();
            }
        }
    }
    private void SetCost()
    {

        for (int i = 0; i < costText.Length; i++)
        {
            costText[i].text = upgradeCost[i].ToString();
        }
    }
    private void ResetLevels()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i] = startLevels[i];

            if (levels[i] != UpgradeManager.instance.tac14Caps[i])
            {
                levelText[i].text = levels[i].ToString();
            }

        }
    }

}
