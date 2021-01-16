using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LerpFunctions;
using UnityEngine.UI;

public class EndResultManager : MonoBehaviour
{
    [System.Serializable]
    public struct RewardType
    {
        public CurrencyType type;
        public int amount;
    }

    public static EndResultManager instance;

    SceneTransitionManager sceneManager;
    DatabaseManager databaseManager;

    [Header("References")]
    public CanvasGroup endingOverlay;
    public RectTransform resultRT;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI rewardText;
    public Image rewardIcon;
    public float transitionSpeed = 15f;

    [Header("Settings")]
    public string victoryText;
    public Color victoryColor;
    public string defeatText;
    public Color defeatColor;
    public bool centerLerp = true;
    public string nextLevelName = "MainMenuScene";

    FlexibleRectCorners resultFRC;
    CanvasGroupFader endingCGF;
    bool playerVictory = false;

    [Header("Rewards")]
    public IconData normalCurrencyIcon;
    public IconData premiumCurrencyIcon;
    public RewardType victoryReward;
    public RewardType defeatReward;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        sceneManager = SceneTransitionManager.instance;
        databaseManager = DatabaseManager.instance;

        resultFRC = new FlexibleRectCorners(resultRT);
        resultFRC.Close();

        endingCGF = new CanvasGroupFader(endingOverlay, true, true);
        endingCGF.fadeEndedEvent.AddListener(UpdateResults);
        endingCGF.SetTransparent();
    }

    void Update()
    {
        endingCGF.Step(transitionSpeed * Time.unscaledDeltaTime);
        resultFRC.CornerStep(transitionSpeed * Time.unscaledDeltaTime);
    }

    public void TriggerResults(bool isVictory)
    {
        playerVictory = isVictory;
        endingCGF.StartFadeIn();
    }

    void UpdateResults()
    {
        if (playerVictory)
        {
            resultText.text = victoryText;
            resultText.color = victoryColor;

            rewardText.text = victoryReward.amount.ToString();
            databaseManager.AddCurrency(victoryReward.amount, victoryReward.type);

            if (victoryReward.type == CurrencyType.NORMAL_CURRENCY) SwapIcons(normalCurrencyIcon);
            else if (victoryReward.type == CurrencyType.PREMIUM_CURRENCY) SwapIcons(premiumCurrencyIcon);
        }
        else
        {
            resultText.text = defeatText;
            resultText.color = defeatColor;

            rewardText.text = defeatReward.amount.ToString();
            databaseManager.AddCurrency(defeatReward.amount, defeatReward.type);

            if (defeatReward.type == CurrencyType.NORMAL_CURRENCY) SwapIcons(normalCurrencyIcon);
            else if (defeatReward.type == CurrencyType.PREMIUM_CURRENCY) SwapIcons(premiumCurrencyIcon);
        }

        if (centerLerp) resultFRC.StartCenterLerp();
        else resultFRC.StartMiddleLerp();
    }

    void SwapIcons(IconData data)
    {
        rewardIcon.sprite = data.sprite;
        rewardIcon.color = data.spriteColor;
    }

    public void BTN_LoadNextLevel()
    {
        sceneManager.ActivateTransition(nextLevelName);
    }
}
