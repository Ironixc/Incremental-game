using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerCard : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionTextPlus;
    public TextMeshProUGUI descriptionTextMinus;
    public TextMeshProUGUI rarityText; 
    public Button myButton;

    private CardManager dealer;
    private int myPowerID;

    public void SetupCard(string newTitle, string newDescPlus, string newDescMinus, int powerID, CardRarity rarity, CardManager managerScript)
    {
        titleText.text = newTitle;
        descriptionTextPlus.text = newDescPlus;
        descriptionTextMinus.text = newDescMinus;
        myPowerID = powerID;
        dealer = managerScript;

        rarityText.text = rarity.ToString();

        switch (rarity)
        {
            case CardRarity.Common:
                rarityText.color = new Color32(189, 189, 189, 255); 
                break;
            case CardRarity.Rare:
                rarityText.color = new Color32(52, 152, 219, 255);  
                break;
            case CardRarity.Epic:
                rarityText.color = new Color32(155, 89, 182, 255); 
                break;
            case CardRarity.Legendary:
                rarityText.color = new Color32(241, 196, 15, 255);  
                break;
        }

        myButton.onClick.RemoveAllListeners();
        myButton.onClick.AddListener(PickCard);
    }

    void PickCard()
    {
        if (dealer.mainScript.audioManager != null)
        {
            dealer.mainScript.audioManager.PlayClick();
        }

        dealer.CardWasPicked(myPowerID);
    }
}