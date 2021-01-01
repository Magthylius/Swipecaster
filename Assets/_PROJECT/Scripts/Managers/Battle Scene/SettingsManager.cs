using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    SceneTransitionManager stManager;

    [Header("Setup")]
    public Image meterLeft1;
    public Image meterLeft2;
    public Image meterRight1;
    public Image meterRight2;
    public CanvasGroup settingsCanvas;
    public string mainMenuScenePath;

    [Header("Settings")]
    public float chargeSpeed = 1f;
    public float transitionSpeed = 5f;
    float trueCharge = 0f;

    bool settingsActivated = false;
    bool settingsTransition = false;
    bool isCharging = false;
    bool allowCharge = false;

    float maxCharge = 1.0f;
    float minCharge = 0.0f;

    [HideInInspector]
    public UnityEvent pausedEvent;
    public UnityEvent unpausedEvent;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        stManager = SceneTransitionManager.instance;

        meterLeft1.fillAmount = 0f;
        meterLeft2.fillAmount = 0f;
        meterRight1.fillAmount = 0f;
        meterRight2.fillAmount = 0f;

        settingsCanvas.alpha = 0.0f;
        settingsCanvas.blocksRaycasts = false;
        settingsCanvas.interactable = false;

        settingsTransition = false;
    }

    void Update()
    {
        if (settingsTransition)
        {
            if (settingsActivated)
            {
                settingsCanvas.alpha = Mathf.Lerp(settingsCanvas.alpha, 1.0f, transitionSpeed * Time.unscaledDeltaTime);

                if (1.0f - settingsCanvas.alpha <= 0.01f)
                {
                    settingsCanvas.alpha = 1.0f;
                    settingsCanvas.blocksRaycasts = true;
                    settingsCanvas.interactable = true;

                    settingsTransition = false;
                }
            }
            else
            {
                settingsCanvas.alpha = Mathf.Lerp(settingsCanvas.alpha, 0.0f, transitionSpeed * Time.unscaledDeltaTime);

                if (settingsCanvas.alpha - 0f <= 0.01f)
                {
                    settingsCanvas.alpha = 0.0f;
                    settingsCanvas.blocksRaycasts = false;
                    settingsCanvas.interactable = false;

                    settingsTransition = false;
                }
            }
        }

        if (allowCharge && settingsActivated)
        {
            if (isCharging)
            {
                trueCharge = Mathf.Lerp(trueCharge, maxCharge, chargeSpeed * Time.unscaledDeltaTime);

                if (LerpFunctions.Lerp.NegligibleDistance(trueCharge, maxCharge, 0.001f))
                {
                    allowCharge = false;
                    trueCharge = maxCharge;

                    stManager.ActivateTransition(mainMenuScenePath);
                }
            }
            else
            {
                trueCharge = Mathf.Lerp(trueCharge, minCharge, chargeSpeed * Time.unscaledDeltaTime);

                if (LerpFunctions.Lerp.NegligibleDistance(trueCharge, minCharge, 0.001f))
                {
                    allowCharge = false;
                    trueCharge = minCharge;
                }
            }

            meterLeft1.fillAmount = trueCharge;
            meterLeft2.fillAmount = trueCharge;
            meterRight1.fillAmount = trueCharge;
            meterRight2.fillAmount = trueCharge;
        }
    }

    public void PNTR_Down()
    {
        isCharging = true;
        allowCharge = true;
    }

    public void PNTR_Up()
    {
        isCharging = false;
        allowCharge = true;
    }

    public void BTN_ActivateSettings()
    {
        settingsActivated = true;
        settingsTransition = true;

        meterLeft1.fillAmount = 0f;
        meterLeft2.fillAmount = 0f;
        meterRight1.fillAmount = 0f;
        meterRight2.fillAmount = 0f;

        Time.timeScale = 0f;
        pausedEvent.Invoke();
    }

    public void BTN_DeactivateSettings()
    {
        settingsActivated = false;
        settingsTransition = true;

        Time.timeScale = 1f;
        unpausedEvent.Invoke();
    }

    #region Accessors
    public bool GetPaused() => settingsActivated;
    #endregion
}
