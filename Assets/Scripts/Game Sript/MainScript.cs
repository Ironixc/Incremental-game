using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    [Header("Prestige / Skill Tree")]
    public int totalSkillPoints;
    public int goalsReachedThisRun; 
    public float permanentGlobalMultiplier = 1f; 
    [Header("Card Buff Modifiers")]
    public float globalGoldMultiplier = 1f;
    public float baseGoldIncrement = 1f;
    public float boosterIncrement = 0.1f;
    public float compoundingIncrement = 0.01f;

    public TextMeshProUGUI GoldText;
    public int Gold;
    public float BaseGold = 1;
    public int CompletedGoals = 0;

    [Header("Upgrade Button Connections")]
    public UpgradeItem upgradeDouble;
    public UpgradeItem upgradeBase;
    public UpgradeItem upgradeExponent;
    public UpgradeItem upgradeBoost;

    [Header("Game Speed Settings")]
    public TextMeshProUGUI SpeedButtonText;
    public float[] availableSpeeds = { 1f, 2f, 5f };
    private int currentSpeedIndex = 0;

    public AudioManager audioManager;

    public float doubleBoost = 1f;
    public float exponentBoost = 1f;
    public float boostMultiplier = 1f;

    public bool isGameOver = false;

    private float eachSecond = 0f;
    public Dictionary<string, int> Inventory = new Dictionary<string, int>();

    void Start()
    {
        // LOAD saved prestige data from the hard drive when the scene starts
        totalSkillPoints = PlayerPrefs.GetInt("SavedSkillPoints", 0);
        permanentGlobalMultiplier = PlayerPrefs.GetFloat("SavedMultiplier", 1f);

        CalculateBoosts();
        UpdateSpeedUI();
    }

    void Update()
    {
        eachSecond += Time.deltaTime;

        while (eachSecond >= 1f && !isGameOver)
        {
            eachSecond -= 1f;
            GoldGains();
        }

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

        // ADDED: * permanentGlobalMultiplier at the very end to apply the skill tree boost!
        BaseGold = (baseGoldIncrement + (boostMultiplier * level2)) * globalGoldMultiplier * permanentGlobalMultiplier;

        doubleBoost = (int)Mathf.Pow(2, level1) * boostMultiplier;
    }

    private void GoldGains()
    {
        float calculatedGold = BaseGold * doubleBoost;
        calculatedGold = Mathf.Pow(calculatedGold, exponentBoost);
        Gold += (int)calculatedGold;
    }

    public void ToggleGameSpeed()
    {
        currentSpeedIndex++;

        // Loop back to 1x speed after 5x
        if (currentSpeedIndex >= availableSpeeds.Length)
        {
            currentSpeedIndex = 0;
        }

        if (audioManager != null)
        {
            audioManager.PlayClick();
        }

        UpdateSpeedUI();

        if (Time.timeScale > 0f)
        {
            Time.timeScale = availableSpeeds[currentSpeedIndex];
        }
    }

    public float GetCurrentSpeed()
    {
        return availableSpeeds[currentSpeedIndex];
    }

    private void UpdateSpeedUI()
    {
        if (SpeedButtonText != null)
        {
            SpeedButtonText.text = availableSpeeds[currentSpeedIndex] + "x Speed";
        }
    }
    public void EndRunAndCalculatePrestige()
    {
        int pointsEarned = goalsReachedThisRun / 1;
        totalSkillPoints += pointsEarned;

        PlayerPrefs.SetInt("SavedSkillPoints", totalSkillPoints);
        PlayerPrefs.Save();

        Debug.Log("Run Ended! Earned " + pointsEarned + " Skill Points. Total: " + totalSkillPoints);
    }

    public void RebirthAndStartNewRun()
    {
        Gold = 0;
        goalsReachedThisRun = 0;

        UpgradeItem[] allStandardUpgrades = FindObjectsByType<UpgradeItem>(FindObjectsSortMode.None);
        foreach (UpgradeItem upgrade in allStandardUpgrades)
        {
            upgrade.currentLevel = 0;


            upgrade.UpdateUI();
        }

        CalculateBoosts();
    }
}

