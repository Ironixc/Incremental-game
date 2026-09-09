using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public Upgrade UpgradeCoins;

    public TextMeshProUGUI GoldText;

    public int Gold;
    public float BaseGold = 1;

    private float TimePlayed = 0f;
    private float eachSecond = 0f;

    public Dictionary<string, int> Inventory = new Dictionary<string, int>();

    void Start()
    {
    }
    void Update() 
    {
        TimePlayed += Time.deltaTime;
        eachSecond += Time.deltaTime;
        if (eachSecond >= 1f) {
            eachSecond = 0f;
            GoldGains();
        }
    }

    private void GoldGains() 
    {
        float calculatedGold = BaseGold * UpgradeCoins.Upgrade_Gold_1_double_Gold_Upgrades_Boost;
        calculatedGold = Mathf.Pow(calculatedGold, UpgradeCoins.Upgrade_Gold_3_exponent_Gold_Upgrades_Boost);
        Gold += (int)calculatedGold;
        GoldText.text = Gold.ToString();
    }
}
