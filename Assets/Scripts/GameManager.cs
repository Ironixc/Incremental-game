using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Global Scripts")]
    public MainScript mainScript;
    public AudioManager audioManager;
    public CardManager cardManager;
    public Goals goalsManager;
    public Sprites spritesManager;
    void Awake()
    {
        Instance = this;
    }
}