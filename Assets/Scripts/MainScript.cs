using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScript : MonoBehaviour
{
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

    // Make sure to drag your AudioManager game object into this slot in the Unity Inspector!
    public AudioManager audioManager;

    public float doubleBoost = 1f;
    public float exponentBoost = 1f;
    public float boostMultiplier = 1f;

    public bool isGameOver = false;

    private float eachSecond = 0f;
    public Dictionary<string, int> Inventory = new Dictionary<string, int>();

    void Start()
    {
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

        // FIXED: Removed the minus signs and added the * symbols
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
}