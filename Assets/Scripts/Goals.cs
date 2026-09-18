using UnityEngine;
using TMPro;
using System;

public class Goals : MonoBehaviour
{
    public TextMeshProUGUI Timer_Text;
    public TextMeshProUGUI Goal_Text;
    public TextMeshProUGUI CompletedGoals_text;

    public GameObject GameOver;

    public MainScript mainScript;
    public CardManager cardManager; // <-- NEW: We need to link the Dealer here!

    public float RemainingTime;
    public int Goal = 50;
    public int GoalMultiplier = 3;
    void start()
    {
        Goal_Text.text = "Goal: " + Goal;
    }
    void Update()
    {
        // 1. Check if the player has enough gold to hit the goal
        if (mainScript.Gold >= Goal)
        {
            // Call the Manager to show the popup
            cardManager.ShowCards();

            // Multiply the goal for the next milestone (e.g., 100 becomes 300)
            Goal = Goal * GoalMultiplier;

            mainScript.CompletedGoals++;
            Goal_Text.text = "Goal: " + Goal;

        }

        if (RemainingTime > 0)
        {
            RemainingTime -= Time.deltaTime;
        }
        else if (RemainingTime <= 0)
        {
            RemainingTime = 0;
            CompletedGoals_text.text = "Completed Goals: " + mainScript.CompletedGoals;
            GameOver.SetActive(true);
        }

        int minutes = Mathf.FloorToInt(RemainingTime / 60);
        int seconds = Mathf.FloorToInt(RemainingTime % 60);
        Timer_Text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}