using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    [Header("Card Buff Modifiers")]
    public float globalGoldMultiplier = 1f; // For the Epic 1.25x/1.5x boosts
    public float baseGoldIncrement = 1f;    // The base increment for More Bronze
    public float boosterIncrement = 0.1f;   // The 0.1f for the Booster math
    public float compoundingIncrement = 0.01f; // The 0.01f for Exponent math

    public TextMeshProUGUI GoldText;
    public int Gold;
    public float BaseGold = 1;
    public int CompletedGoals = 0;

    [Header("Upgrade Button Connections")]
    public UpgradeItem upgradeDouble;   
    public UpgradeItem upgradeBase;     
    public UpgradeItem upgradeExponent; 
    public UpgradeItem upgradeBoost;    

    public float doubleBoost = 1f;
    public float exponentBoost = 1f;
    public float boostMultiplier = 1f;

    private float eachSecond = 0f;
    public Dictionary<string, int> Inventory = new Dictionary<string, int>();

    void Start()
    {
        CalculateBoosts(); // Run math once at start
    }

    void Update()
    {
        eachSecond += Time.deltaTime;
        if (eachSecond >= 1f)
        {
            eachSecond = 0f;
            GoldGains();
        }

        // Update gold text in Update so purchases reflect instantly
        GoldText.text = Gold.ToString();
    }

    public void CalculateBoosts()
    {
        int level1 = upgradeDouble.currentLevel;
        int level2 = upgradeBase.currentLevel;
        int level3 = upgradeExponent.currentLevel;
        int level4 = upgradeBoost.currentLevel;

        boostMultiplier = 1f + (level4 * boosterIncrement);
        exponentBoost = 1f + ((level3 * compoundingIncrement) * boostMultiplier);
        BaseGold = (baseGoldIncrement + (boostMultiplier * level2)) * globalGoldMultiplier;
        doubleBoost = (int)Mathf.Pow(2, level1) * boostMultiplier;
    }

    private void GoldGains()
    {
        float calculatedGold = BaseGold * doubleBoost;
        calculatedGold = Mathf.Pow(calculatedGold, exponentBoost);
        Gold += (int)calculatedGold;
    }
}