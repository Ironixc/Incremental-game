using UnityEngine;
using System.Collections.Generic;

// Define our rarities
public enum CardRarity { Common, Rare, Epic, Legendary }

[System.Serializable]
public class PowerData
{
    public string title;
    public string descriptionPlus;
    public string descriptionMinus;
    public int powerID;
    public CardRarity rarity;
}

public class CardManager : MonoBehaviour
{
    public MainScript mainScript;
    public Goals levelTimer;
    public GameObject popupPanel;
    public PowerCard[] cardSlots;

    [HideInInspector]
    public List<PowerData> allPossiblePowers;

    private void Awake()
    {
        allPossiblePowers = new List<PowerData>
        {
            // === COMMON CARDS ===
            new PowerData { powerID = 1, title = "Base Upgrader", descriptionPlus = "Max Level + 1", descriptionMinus = "-", rarity = CardRarity.Common },
            new PowerData { powerID = 2, title = "Upgrade Double", descriptionPlus = "Max Level + 2", descriptionMinus = "Cost x 1.1", rarity = CardRarity.Common },
            new PowerData { powerID = 3, title = "Upgrade Exponent", descriptionPlus = "+ 1 Level", descriptionMinus = "Time -30s", rarity = CardRarity.Common },
            new PowerData { powerID = 4, title = "Upgrade Boost", descriptionPlus = "+ 1 Level", descriptionMinus = "All Costs x 1.05", rarity = CardRarity.Common },
            new PowerData { powerID = 6, title = "Base Increment", descriptionPlus = "Increment + 0.1", descriptionMinus = "Time -90s", rarity = CardRarity.Common },
            new PowerData { powerID = 7, title = "Booster Increment", descriptionPlus = "Increment + 0.005", descriptionMinus = "All Costs x 1.1", rarity = CardRarity.Common },
            new PowerData { powerID = 8, title = "Small Time", descriptionPlus = "Time + 30s", descriptionMinus = "Goal Multiplier + 0.1", rarity = CardRarity.Common },
            new PowerData { powerID = 9, title = "Medium Time", descriptionPlus = "Time + 240s", descriptionMinus = "Goal Multiplier + 0.5", rarity = CardRarity.Rare },

            // === RARE CARDS ===
            new PowerData { powerID = 10, title = "Rare Base", descriptionPlus = "Max Level + 10", descriptionMinus = "-", rarity = CardRarity.Rare },
            new PowerData { powerID = 11, title = "Rare Double", descriptionPlus = "Max Level + 7", descriptionMinus = "Goal Multiplier + 0.1", rarity = CardRarity.Rare },
            new PowerData { powerID = 12, title = "Rare Compounding", descriptionPlus = "Max Level + 2", descriptionMinus = "Time -30s", rarity = CardRarity.Rare },
            new PowerData { powerID = 13, title = "Rare ", descriptionPlus = "Max Level + 3", descriptionMinus = "All Costs x 1.05", rarity = CardRarity.Rare },
            new PowerData { powerID = 14, title = "Global Boost", descriptionPlus = "Global x 1.1", descriptionMinus = "Base Cost x 1.5", rarity = CardRarity.Rare },
            new PowerData { powerID = 15, title = "Rare Time", descriptionPlus = "Time + 30s", descriptionMinus = "-", rarity = CardRarity.Rare },
            new PowerData { powerID = 16, title = "Big Global Boost", descriptionPlus = "Global x 1.25", descriptionMinus = "Boost Cost x 1.5", rarity = CardRarity.Rare },
        };
    }

    public void ShowCards()
    {
        popupPanel.SetActive(true);
        Time.timeScale = 0f;

        List<PowerData> tempDeck = new List<PowerData>(allPossiblePowers);

        for (int i = 0; i < 3; i++)
        {
            int roll = Random.Range(0, 100);
            CardRarity targetRarity;

            if (roll < 71) targetRarity = CardRarity.Common;
            else targetRarity = CardRarity.Rare;

            List<PowerData> validCards = tempDeck.FindAll(card => card.rarity == targetRarity);

            if (validCards.Count == 0)
            {
                validCards = tempDeck;
            }

            int randomIndex = Random.Range(0, validCards.Count);
            PowerData pickedPower = validCards[randomIndex];

            cardSlots[i].SetupCard(pickedPower.title, pickedPower.descriptionPlus, pickedPower.descriptionMinus, pickedPower.powerID, pickedPower.rarity, this);

            tempDeck.Remove(pickedPower);
        }
    }

    public void CardWasPicked(int chosenPowerID)
    {
        switch (chosenPowerID)
        {
            case 1:
                mainScript.upgradeBase.maxLevel += 1;
                mainScript.upgradeBase.UpdateUI();
                break;
            case 2:
                mainScript.upgradeDouble.maxLevel += 1;
                mainScript.upgradeDouble.costMultiplier *= 1.1f;
                mainScript.upgradeDouble.UpdateUI();
                break;
            case 3:
                mainScript.upgradeExponent.currentLevel += 1;
                levelTimer.RemainingTime -= 60f;
                mainScript.upgradeExponent.UpdateUI();
                break;
            case 4:
                mainScript.upgradeBoost.currentLevel += 1;
                MultiplyAllCosts(1.05f);
                mainScript.upgradeBoost.UpdateUI();
                break;
            case 6:
                mainScript.baseGoldIncrement += 0.1f;
                levelTimer.RemainingTime -= 90f;
                mainScript.CalculateBoosts();
                break;
            case 7:
                mainScript.boosterIncrement += 0.005f;
                MultiplyAllCosts(1.1f);
                mainScript.CalculateBoosts();
                break;
            case 8:
                levelTimer.RemainingTime += 60f;
                levelTimer.GoalMultiplier += 0.1f;
                break;
            case 9:
                levelTimer.RemainingTime += 240f;
                levelTimer.GoalMultiplier += 0.5f;
                break;

            // === RARE CARDS ===
            case 10:
                mainScript.upgradeBase.maxLevel += 10;
                mainScript.upgradeBase.costMultiplier *= 1.3f;
                mainScript.upgradeBase.UpdateUI();
                break;

            // --- FIX FOR CASE 11 APPLIED HERE ---
            case 11:
                mainScript.upgradeDouble.maxLevel += 7;
                levelTimer.GoalMultiplier += 0.1f;
                mainScript.upgradeDouble.UpdateUI();
                break;

            case 12:
                mainScript.upgradeExponent.maxLevel += 3;
                levelTimer.RemainingTime -= 30f;
                levelTimer.GoalMultiplier += 0.5f;
                mainScript.upgradeExponent.UpdateUI();
                break;
            case 13:
                mainScript.upgradeBoost.maxLevel += 3;
                MultiplyAllCosts(1.05f);
                mainScript.upgradeBoost.UpdateUI();
                break;
            case 14:
                levelTimer.GoalMultiplier -= 1;
                levelTimer.RemainingTime -= 60f;
                break;
            case 15:
                levelTimer.RemainingTime += 30f;
                break;
        }

        popupPanel.SetActive(false);

        Time.timeScale = mainScript.GetCurrentSpeed();
    }

    private void MultiplyAllCosts(float multiplierAmount)
    {
        mainScript.upgradeDouble.costMultiplier *= multiplierAmount;
        mainScript.upgradeBase.costMultiplier *= multiplierAmount;
        mainScript.upgradeExponent.costMultiplier *= multiplierAmount;
        mainScript.upgradeBoost.costMultiplier *= multiplierAmount;
    }
}