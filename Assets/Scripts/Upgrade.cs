using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    public MainScript mainScript;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI LevelText;
    public Button upgradeButton;

    [Header("Visuals")]
    public Image buttonImage; // The background image of the button
    public Sprite defaultSprite; // The normal bronze look
    public Sprite lockedSprite;  // The crossed-out locked look

    [Header("Stats")]
    public int currentCost = 10;
    public float costMultiplier = 1.5f;
    public int currentLevel = 0;
    public int maxLevel = 5; // Set this differently for each button in Unity!

    void Start()
    {
        UpdateUI();
        upgradeButton.onClick.AddListener(BuyUpgrade);
    }

    public void BuyUpgrade()
    {
        // Stop the player from buying if they are already at max level
        if (currentLevel >= maxLevel) return;

        if (mainScript.Gold >= currentCost)
        {
            mainScript.Gold -= currentCost;
            currentCost = (int)(currentCost * costMultiplier);
            currentLevel++;

            UpdateUI();
            mainScript.CalculateBoosts();
        }
    }

    // Made this public so the CardManager can update the UI if the cap increases!
    public void UpdateUI()
    {
        LevelText.text = "" + currentLevel;

        if (currentLevel >= maxLevel)
        {
            CostText.text = "MAX";
            buttonImage.sprite = lockedSprite; // Change to the X sprite
            upgradeButton.interactable = false; // Optional: stops the clicking animation
        }
        else
        {
            CostText.text = "" + currentCost;
            buttonImage.sprite = defaultSprite; // Keep the normal bronze sprite
            upgradeButton.interactable = true;
        }
    }
}