using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LerpFunctions;

public class EndResultManager : MonoBehaviour
{
    public static EndResultManager instance;

    SceneTransitionManager sceneManager;

    [Header("References")]
    public CanvasGroup endingOverlay;
    public TextMeshProUGUI resultText;
    public RectTransform resultRT;
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

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        sceneManager = SceneTransitionManager.instance;

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
        }
        else
        {
            resultText.text = defeatText;
            resultText.color = defeatColor;
        }

        if (centerLerp) resultFRC.StartCenterLerp();
        else resultFRC.StartMiddleLerp();
    }

    public void BTN_LoadNextLevel()
    {
        sceneManager.ActivateTransition(nextLevelName);
    }
}
