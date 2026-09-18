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
    public Goals levelTimer; // <-- NEW: Drag your Goals object here in Unity!
    public GameObject popupPanel;
    public PowerCard[] cardSlots;

    [Header("All Game Powers (Main Deck)")]
    public List<PowerData> allPossiblePowers;

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
            else if (roll < 98) targetRarity = CardRarity.Rare;
            else targetRarity = CardRarity.Epic;

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
            // === COMMON CARDS ===
            case 1: // M + 10, C x 1.1
                mainScript.upgradeBase.maxLevel += 10;
                mainScript.upgradeBase.costMultiplier *= 1.1f;
                mainScript.upgradeBase.UpdateUI();
                break;
            case 2: // Double + 3, C x 1.15
                mainScript.upgradeDouble.maxLevel += 3;
                mainScript.upgradeDouble.costMultiplier *= 1.15f;
                mainScript.upgradeDouble.UpdateUI();
                break;
            case 3: // Compounding + 1, Time - 30s
                mainScript.upgradeExponent.maxLevel += 1;
                levelTimer.RemainingTime -= 30f;
                mainScript.upgradeExponent.UpdateUI();
                break;
            case 4: // Booster + 1, All Cost x 1.05
                mainScript.upgradeBoost.maxLevel += 1;
                MultiplyAllCosts(1.05f); // Uses helper method at bottom
                mainScript.upgradeBoost.UpdateUI();
                break;
            case 5: // More Bronze + 2, Time - 90s
                mainScript.upgradeBase.maxLevel += 2;
                levelTimer.RemainingTime -= 90f;
                mainScript.upgradeBase.UpdateUI();
                break;
            case 6: // Base Increment + 0.1, Time - 180s
                mainScript.baseGoldIncrement += 0.1f;
                levelTimer.RemainingTime -= 180f;
                mainScript.CalculateBoosts(); // Recalculate immediately
                break;
            case 7: // Booster Increment + 0.005, All Cost x 1.1
                mainScript.boosterIncrement += 0.005f;
                MultiplyAllCosts(1.1f);
                mainScript.CalculateBoosts();
                break;
            case 8: // Time + 30s
                levelTimer.RemainingTime += 30f;
                break;
            case 9: // Time + 240s, All Cost x 1.1
                levelTimer.RemainingTime += 240f;
                MultiplyAllCosts(1.1f);
                break;

            // === RARE CARDS ===
            case 10: // M + 10, C x 1.05
                mainScript.upgradeBase.maxLevel += 10;
                mainScript.upgradeBase.costMultiplier *= 1.05f;
                mainScript.upgradeBase.UpdateUI();
                break;
            case 11: // Double + 5, C x 1.15
                mainScript.upgradeDouble.maxLevel += 5;
                mainScript.upgradeDouble.costMultiplier *= 1.15f;
                mainScript.upgradeDouble.UpdateUI();
                break;
            case 12: // Compounding + 2, Time - 30s
                mainScript.upgradeExponent.maxLevel += 2;
                levelTimer.RemainingTime -= 30f;
                mainScript.upgradeExponent.UpdateUI();
                break;
            case 13: // Booster + 3, All Cost x 1.05
                mainScript.upgradeBoost.maxLevel += 3;
                MultiplyAllCosts(1.05f);
                mainScript.upgradeBoost.UpdateUI();
                break;
            case 14: // Get 1.1x Global Boost, More Bronze Cost x 1.5
                mainScript.globalGoldMultiplier *= 1.1f;
                mainScript.upgradeBase.costMultiplier *= 1.5f;
                mainScript.CalculateBoosts();
                mainScript.upgradeBase.UpdateUI();
                break;
            case 15: // Time + 60s
                levelTimer.RemainingTime += 60f;
                break;
            case 16: // Get 1.25x Global Boost, Booster Cost x 1.5
                mainScript.globalGoldMultiplier *= 1.25f;
                mainScript.upgradeBoost.costMultiplier *= 1.5f;
                mainScript.CalculateBoosts();
                mainScript.upgradeBoost.UpdateUI();
                break;
            case 17: // Time + 600s, Compounding penalty
                levelTimer.RemainingTime += 600f;
                // Note: I changed -0.05 to -0.005 so your math doesn't break into negatives!
                mainScript.compoundingIncrement -= 0.005f;
                mainScript.CalculateBoosts();
                break;

            // === EPIC CARDS ===
            case 18: // All Caps + 1 Free
                mainScript.upgradeDouble.maxLevel += 1;
                mainScript.upgradeBase.maxLevel += 1;
                mainScript.upgradeExponent.maxLevel += 1;
                mainScript.upgradeBoost.maxLevel += 1;

                mainScript.upgradeDouble.UpdateUI();
                mainScript.upgradeBase.UpdateUI();
                mainScript.upgradeExponent.UpdateUI();
                mainScript.upgradeBoost.UpdateUI();
                break;
            case 19: // Time + 240 Free
                levelTimer.RemainingTime += 240f;
                break;
            case 20: // Reduce All Costs by 20%
                MultiplyAllCosts(0.80f); // 0.8 is a 20% discount

                // Immediately discount the CURRENT price tag on the screen too
                mainScript.upgradeDouble.currentCost = (int)(mainScript.upgradeDouble.currentCost * 0.8f);
                mainScript.upgradeBase.currentCost = (int)(mainScript.upgradeBase.currentCost * 0.8f);
                mainScript.upgradeExponent.currentCost = (int)(mainScript.upgradeExponent.currentCost * 0.8f);
                mainScript.upgradeBoost.currentCost = (int)(mainScript.upgradeBoost.currentCost * 0.8f);

                mainScript.upgradeDouble.UpdateUI();
                mainScript.upgradeBase.UpdateUI();
                mainScript.upgradeExponent.UpdateUI();
                mainScript.upgradeBoost.UpdateUI();
                break;
            case 21: // Global Boost x 1.5
                mainScript.globalGoldMultiplier *= 1.5f;
                mainScript.CalculateBoosts();
                break;
            case 22: // The 22nd Epic Card! (Time + 600, All Caps + 2, Costs x2)
                levelTimer.RemainingTime += 600f;
                mainScript.upgradeDouble.maxLevel += 2;
                mainScript.upgradeBase.maxLevel += 2;
                mainScript.upgradeExponent.maxLevel += 2;
                mainScript.upgradeBoost.maxLevel += 2;
                MultiplyAllCosts(2f);

                mainScript.upgradeDouble.UpdateUI();
                mainScript.upgradeBase.UpdateUI();
                mainScript.upgradeExponent.UpdateUI();
                mainScript.upgradeBoost.UpdateUI();
                break;
        }

        popupPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void MultiplyAllCosts(float multiplierAmount)
    {
        mainScript.upgradeDouble.costMultiplier *= multiplierAmount;
        mainScript.upgradeBase.costMultiplier *= multiplierAmount;
        mainScript.upgradeExponent.costMultiplier *= multiplierAmount;
        mainScript.upgradeBoost.costMultiplier *= multiplierAmount;
    }
}