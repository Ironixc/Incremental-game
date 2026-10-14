using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    public MainScript mainScript;
    private TextMeshProUGUI CostText;
    public TextMeshProUGUI LevelText;
    private TextMeshProUGUI InfoText;
    public Button upgradeButton;

    [Header("Visuals")]
    public Image buttonImage;
    private Sprite defaultSprite;
    private Sprite lockedSprite;

    [Header("Animation")]
    private Sprite[] clickFrames;

    private float[] textYOffsets;
    private RectTransform[] textElementsToMove;
    private float animSpeed = 0.03f;

    private Coroutine currentAnim;
    private Vector2[] originalPositions;

    [Header("Stats")]
    public int currentCost = 10;
    public float costMultiplier = 1.5f;
    public int currentLevel = 0;
    public int maxLevel = 5;

    public AudioManager audioManager;

    void Start()
    {
        defaultSprite = Resources.Load<Sprite>("Bronze Button");
        lockedSprite = Resources.Load<Sprite>("Lock Bronze Button");
        Transform costTransform = transform.parent.Find("Cost");
        CostText = costTransform.GetComponent<TextMeshProUGUI>();
        Transform infoTransform = transform.parent.Find("Info");
        InfoText = infoTransform.GetComponent<TextMeshProUGUI>();
        Sprite[] allSprites = Resources.LoadAll<Sprite>("Bronze Button Animation");

        clickFrames = new Sprite[5];

        foreach (Sprite spr in allSprites)
        {
            if (spr.name == "Frame_0") clickFrames[0] = spr;
            else if (spr.name == "Frame_1") clickFrames[1] = spr;
            else if (spr.name == "Frame_2") clickFrames[2] = spr;
            else if (spr.name == "Frame_3") clickFrames[3] = spr;
            else if (spr.name == "Frame_4") clickFrames[4] = spr;
        }

        textYOffsets = new float[] { 0f, -2f, -4f, -2f, 0f };
        textElementsToMove = new RectTransform[] { CostText.rectTransform, InfoText.rectTransform };

        UpdateUI();
        upgradeButton.onClick.AddListener(BuyUpgrade);

        if (textElementsToMove != null)
        {
            originalPositions = new Vector2[textElementsToMove.Length];
            for (int i = 0; i < textElementsToMove.Length; i++)
            {
                if (textElementsToMove[i] != null)
                {
                    originalPositions[i] = textElementsToMove[i].anchoredPosition;
                }
            }
        }
    }

    public void BuyUpgrade()
    {
        if (currentLevel >= maxLevel) return;

        if (currentAnim != null) StopCoroutine(currentAnim);
        currentAnim = StartCoroutine(PlayClickAnimation());

        if (mainScript.Gold >= currentCost)
        {
            mainScript.Gold -= currentCost;
            currentCost = (int)(currentCost * costMultiplier);
            currentLevel++;

            UpdateUI();
            mainScript.CalculateBoosts();
        }

        if (audioManager != null)
        {
            audioManager.PlayClick();
        }
    }

    public void UpdateUI()
    {
        LevelText.text = "" + currentLevel;

        if (currentLevel >= maxLevel)
        {
            CostText.text = "MAX";
            upgradeButton.interactable = false;
            if (currentAnim == null) buttonImage.sprite = lockedSprite;
        }
        else
        {
            CostText.text = "" + currentCost;
            upgradeButton.interactable = true;
            if (currentAnim == null) buttonImage.sprite = defaultSprite;
        }
    }

    private IEnumerator PlayClickAnimation()
    {
        for (int i = 0; i < clickFrames.Length; i++)
        {
            buttonImage.sprite = clickFrames[i];
            float currentOffset = (textYOffsets != null && i < textYOffsets.Length) ? textYOffsets[i] : 0f;

            if (textElementsToMove != null && originalPositions != null)
            {
                for (int j = 0; j < textElementsToMove.Length; j++)
                {
                    if (textElementsToMove[j] != null)
                    {
                        textElementsToMove[j].anchoredPosition = originalPositions[j] + new Vector2(0, currentOffset);
                    }
                }
            }

            yield return new WaitForSeconds(animSpeed);
        }

        currentAnim = null;

        if (currentLevel >= maxLevel)
        {
            buttonImage.sprite = lockedSprite;
        }
        else
        {
            buttonImage.sprite = defaultSprite;
        }

        if (textElementsToMove != null && originalPositions != null)
        {
            for (int j = 0; j < textElementsToMove.Length; j++)
            {
                if (textElementsToMove[j] != null)
                {
                    textElementsToMove[j].anchoredPosition = originalPositions[j];
                }
            }
        }
    }
}