using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerCard : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionTextPlus;
    public TextMeshProUGUI descriptionTextMinus;
    public TextMeshProUGUI rarityText; // <-- NEW: Slot for your rarity text
    public Button myButton;

    private CardManager dealer;
    private int myPowerID;

    // Notice we added 'CardRarity rarity' to the arguments here so the Manager can hand it over!
    public void SetupCard(string newTitle, string newDescPlus, string newDescMinus, int powerID, CardRarity rarity, CardManager managerScript)
    {
        titleText.text = newTitle;
        descriptionTextPlus.text = newDescPlus;
        descriptionTextMinus.text = newDescMinus;
        myPowerID = powerID;
        dealer = managerScript;

        // 1. Change the actual word to match the rarity (e.g., changes placeholder to "Epic")
        rarityText.text = rarity.ToString();

        // 2. Change the color of the text based on the rarity
        switch (rarity)
        {
            case CardRarity.Common:
                rarityText.color = new Color32(189, 189, 189, 255); // Silver/Grey
                break;
            case CardRarity.Rare:
                rarityText.color = new Color32(52, 152, 219, 255);  // Bright Blue
                break;
            case CardRarity.Epic:
                rarityText.color = new Color32(155, 89, 182, 255);  // Purple
                break;
            case CardRarity.Legendary:
                rarityText.color = new Color32(241, 196, 15, 255);  // Gold
                break;
        }

        myButton.onClick.RemoveAllListeners();
        myButton.onClick.AddListener(PickCard);
    }

    void PickCard()
    {
        dealer.CardWasPicked(myPowerID);
    }
}