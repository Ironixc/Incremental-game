using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BronzeSkillTree1 : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI LevelText;
    public Button buyButton;

    [Header("Skill Stats")]
    public string skillName = "Global Income Boost";
    public int currentCost = 1;
    public int costIncrease = 1; // Flat cost increase is usually better for rare skill points
    public int currentLevel = 0;
    public int maxLevel = 5;

    public float multiplierBoostPerLevel = 0.1f;

    void Start()
    {
        buyButton.onClick.AddListener(BuySkill);
        UpdateUI();
    }

    void OnEnable()
    {
        UpdateUI();
    }

    public void BuySkill()
    {
        if (currentLevel >= maxLevel) return;

        if (GameManager.Instance.mainScript.totalSkillPoints >= currentCost)
        {
            GameManager.Instance.mainScript.totalSkillPoints -= currentCost;

            currentCost += costIncrease;
            currentLevel++;

            GameManager.Instance.mainScript.permanentGlobalMultiplier += multiplierBoostPerLevel;

            PlayerPrefs.SetFloat("SavedMultiplier", GameManager.Instance.mainScript.permanentGlobalMultiplier);
            PlayerPrefs.SetInt("SavedSkillPoints", GameManager.Instance.mainScript.totalSkillPoints);
            PlayerPrefs.Save();

            if (GameManager.Instance.audioManager != null)
            {
                GameManager.Instance.audioManager.PlayClick();
            }

            UpdateUI();
            GameManager.Instance.mainScript.CalculateBoosts();
        }
    }

    public void UpdateUI()
    {
        if (TitleText != null) TitleText.text = skillName;
        if (LevelText != null) LevelText.text = "Lvl " + currentLevel;

        if (currentLevel >= maxLevel)
        {
            if (CostText != null) CostText.text = "MAX";
            buyButton.interactable = false;
        }
        else
        {
            if (CostText != null) CostText.text = currentCost + " SP";

            if (GameManager.Instance != null && GameManager.Instance.mainScript != null)
            {
                buyButton.interactable = (GameManager.Instance.mainScript.totalSkillPoints >= currentCost);
            }
        }
    }
}

