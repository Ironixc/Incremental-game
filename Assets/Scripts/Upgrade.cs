using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public MainScript Mainscript;

    public Button Upgrade_Gold_1_double_Gold_Upgrades_Button;
    public TextMeshProUGUI Upgrade_Gold_1_double_Gold_Upgrades_Text;
    private int Upgrade_Gold_1_double_Gold_Upgrades_Cost = 10;
    private int Upgrade_Gold_1_double_Gold_Upgrades_Level = 0;
    public float Upgrade_Gold_1_double_Gold_Upgrades_Boost = 1;

    public Button Upgrade_Gold_2_Increase_gold_base_Button;
    public TextMeshProUGUI Upgrade_Gold_2_Increase_gold_base_Text;
    private int Upgrade_Gold_2_Increase_gold_base_Cost = 10;
    private int Upgrade_Gold_2_Increase_gold_base_Level = 0;


    public Button Upgrade_Gold_3_exponent_Gold_Upgrades_Button;
    public TextMeshProUGUI Upgrade_Gold_3_exponent_Gold_Upgrades_Text;
    private int Upgrade_Gold_3_exponent_Gold_Upgrades_Cost = 10;
    private int Upgrade_Gold_3_exponent_Gold_Upgrades_Level = 0;
    public float Upgrade_Gold_3_exponent_Gold_Upgrades_Boost = 1;

    public Button Upgrade_Gold_4_Boost_Gold_Upgrades_Button;
    public TextMeshProUGUI Upgrade_Gold_4_Boost_Gold_Upgrades_Text;
    private int Upgrade_Gold_4_Boost_Gold_Upgrades_Cost = 10;
    private int Upgrade_Gold_4_Boost_Gold_Upgrades_Level = 0;
    public float Upgrade_Gold_4_Boost_Gold_Upgrades_Boost = 1;

    void Start()
    {
        Upgrade_Gold_1_double_Gold_Upgrades_Text.text = "Level: " + Upgrade_Gold_1_double_Gold_Upgrades_Level + " | Cost: " + Upgrade_Gold_1_double_Gold_Upgrades_Cost;
        Upgrade_Gold_1_double_Gold_Upgrades_Button.onClick.AddListener(UpgradeGold1);
             
        Upgrade_Gold_2_Increase_gold_base_Text.text = "Level: " + Upgrade_Gold_2_Increase_gold_base_Level + " | Cost: " + Upgrade_Gold_2_Increase_gold_base_Cost;
        Upgrade_Gold_2_Increase_gold_base_Button.onClick.AddListener(UpgradeGold2);
        
        Upgrade_Gold_3_exponent_Gold_Upgrades_Text.text = "Level: " + Upgrade_Gold_3_exponent_Gold_Upgrades_Level + " | Cost: " + Upgrade_Gold_3_exponent_Gold_Upgrades_Cost;
        Upgrade_Gold_3_exponent_Gold_Upgrades_Button.onClick.AddListener(UpgradeGold3);

        Upgrade_Gold_4_Boost_Gold_Upgrades_Text.text = "Level: " + Upgrade_Gold_4_Boost_Gold_Upgrades_Level + " | Cost: " + Upgrade_Gold_4_Boost_Gold_Upgrades_Cost;
        Upgrade_Gold_4_Boost_Gold_Upgrades_Button.onClick.AddListener(UpgradeGold4);
    }

    public void UpgradeGold1()
    {
        if (Mainscript.Gold >= Upgrade_Gold_1_double_Gold_Upgrades_Cost)
        {
            Mainscript.Gold -= Upgrade_Gold_1_double_Gold_Upgrades_Cost;
            Upgrade_Gold_1_double_Gold_Upgrades_Cost = (int)(Upgrade_Gold_1_double_Gold_Upgrades_Cost * 1.5f);
            Upgrade_Gold_1_double_Gold_Upgrades_Level++;
            Upgrade_Gold_1_double_Gold_Upgrades_Text.text = "Level: " + Upgrade_Gold_1_double_Gold_Upgrades_Level + " | Cost: " + Upgrade_Gold_1_double_Gold_Upgrades_Cost;
            CalculateBoosts();
        }
    }
    public void UpgradeGold2()
    {
        if (Mainscript.Gold >= Upgrade_Gold_2_Increase_gold_base_Cost)
        {
            Mainscript.Gold -= Upgrade_Gold_2_Increase_gold_base_Cost;
            Upgrade_Gold_2_Increase_gold_base_Cost = (int)(Upgrade_Gold_2_Increase_gold_base_Cost * 1.5f);
            Upgrade_Gold_2_Increase_gold_base_Level++;
            Upgrade_Gold_2_Increase_gold_base_Text.text = "Level: " + Upgrade_Gold_2_Increase_gold_base_Level + " | Cost: " + Upgrade_Gold_2_Increase_gold_base_Cost;
            CalculateBoosts();
        }
    }
    public void UpgradeGold3()
    {
        if (Mainscript.Gold >= Upgrade_Gold_3_exponent_Gold_Upgrades_Cost)
        {
            Mainscript.Gold -= Upgrade_Gold_3_exponent_Gold_Upgrades_Cost;
            Upgrade_Gold_3_exponent_Gold_Upgrades_Cost = (int)(Upgrade_Gold_3_exponent_Gold_Upgrades_Cost * 1.5f);
            Upgrade_Gold_3_exponent_Gold_Upgrades_Level++;
            Upgrade_Gold_3_exponent_Gold_Upgrades_Text.text = "Level: " + Upgrade_Gold_3_exponent_Gold_Upgrades_Level + " | Cost: " + Upgrade_Gold_3_exponent_Gold_Upgrades_Cost;
            CalculateBoosts();
        }
    }

    public void UpgradeGold4()
    {
        if (Mainscript.Gold >= Upgrade_Gold_4_Boost_Gold_Upgrades_Cost)
        {
            Mainscript.Gold -= Upgrade_Gold_4_Boost_Gold_Upgrades_Cost;
            Upgrade_Gold_4_Boost_Gold_Upgrades_Cost = (int)(Upgrade_Gold_4_Boost_Gold_Upgrades_Cost * 1.5f);
            Upgrade_Gold_4_Boost_Gold_Upgrades_Level++;
            Upgrade_Gold_4_Boost_Gold_Upgrades_Text.text = "Level: " + Upgrade_Gold_4_Boost_Gold_Upgrades_Level + " | Cost: " + Upgrade_Gold_4_Boost_Gold_Upgrades_Cost;
            CalculateBoosts();
        }
    }

    private void CalculateBoosts()
    {
        Upgrade_Gold_4_Boost_Gold_Upgrades_Boost = 1 + (Upgrade_Gold_4_Boost_Gold_Upgrades_Level * 0.1f);

        Upgrade_Gold_3_exponent_Gold_Upgrades_Boost = 1 + ((Upgrade_Gold_3_exponent_Gold_Upgrades_Level * 0.01f) * Upgrade_Gold_4_Boost_Gold_Upgrades_Boost);
        Mainscript.BaseGold = 1 + (Upgrade_Gold_4_Boost_Gold_Upgrades_Boost * Upgrade_Gold_2_Increase_gold_base_Level);
        Upgrade_Gold_1_double_Gold_Upgrades_Boost = (int)Mathf.Pow(2, Upgrade_Gold_1_double_Gold_Upgrades_Level) * Upgrade_Gold_4_Boost_Gold_Upgrades_Boost;


    }
}