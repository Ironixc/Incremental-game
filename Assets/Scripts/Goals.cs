using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement; // REQUIRED to reload the scene

public class Goals : MonoBehaviour
{
    public TextMeshProUGUI Timer_Text;
    public TextMeshProUGUI Goal_Text;
    public TextMeshProUGUI CompletedGoals_text;

    public GameObject GameOver;

    public MainScript mainScript;
    public CardManager cardManager;
    public AudioManager audioManager;

    [Header("Stats")]
    public float RemainingTime;
    public float Goal = 10f;
    public float GoalMultiplier = 3f;

    void Start()
    {
        Goal_Text.text = "Goal: " + Goal;
    }

    void Update()
    {
        if (!mainScript.isGameOver)
        {
            if (mainScript.Gold >= Goal)
            {
                cardManager.ShowCards();

                Goal = Goal * GoalMultiplier;

                mainScript.CompletedGoals++;
                Goal_Text.text = "Goal: " + Mathf.RoundToInt(Goal);
                mainScript.Gold = 0;
                RemainingTime += 20f;
                audioManager.GoalSoundPick();
            }

            if (RemainingTime > 0)
            {
                RemainingTime -= Time.deltaTime;
            }

            if (RemainingTime <= 0)
            {
                RemainingTime = 0;
                mainScript.isGameOver = true;
                CompletedGoals_text.text = "Completed Goals: " + mainScript.CompletedGoals;
                GameOver.SetActive(true);
                audioManager.DefeatSoundPick();
            }

            int minutes = Mathf.FloorToInt(RemainingTime / 60);
            int seconds = Mathf.FloorToInt(RemainingTime % 60);
            Timer_Text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}