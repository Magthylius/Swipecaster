using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSelectBehavior : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    public QuestHandler questHandler;
    public Button button;

    public float transitionSpeed = 2f;
    [SerializeField] private float energyCost = 10.0f;

    RectTransform rectTr;
    MapSelectionManager mapManager;
    float originalHeight;
    float activatedHeight;

    bool isActivated = false;
    bool allowTransition = false;

    float precision = 0.1f;

    void Start()
    {
        rectTr = GetComponent<RectTransform>();
        originalHeight = rectTr.rect.height;
        activatedHeight = 2f * originalHeight;

        mapManager = MapSelectionManager.instance;
        button.onClick.AddListener(UpdateQuestHandler);
    }

    void Update()
    {
        if (allowTransition)
        {
            float height = 0f;

            if (isActivated)
            {
                height = Mathf.Lerp(rectTr.sizeDelta.y, activatedHeight, transitionSpeed * Time.unscaledDeltaTime);

                if (activatedHeight - height <= precision)
                {
                    allowTransition = false;
                    height = activatedHeight;
                    mapManager.RemoveActive();
                }
            }
            else
            {
                height = Mathf.Lerp(rectTr.sizeDelta.y, originalHeight, transitionSpeed * Time.unscaledDeltaTime);

                if (height - originalHeight <= precision)
                {
                    allowTransition = false;
                    height = originalHeight;
                    mapManager.RemoveActive();
                }
            }

            //parent.SetLayoutVertical();
            rectTr.sizeDelta = new Vector2(rectTr.sizeDelta.x, height);
            //mapManager.SetLayoutActive(allowTransition);
        }
    }

    public void Reset()
    {
        isActivated = false;
        allowTransition = false;
    }

    void UpdateQuestHandler()
    {
        questHandler.BTN_SetEnergyCost(energyCost);
        questHandler.BTN_SelectLevel(transform.GetSiblingIndex());
    }

    #region Accessors
    public void BTN_Trigger()
    {
        if (isActivated) Deactivate();
        else Activate();
    }

    public void Activate()
    {
        isActivated = true;
        if (!allowTransition) mapManager.AddActive();
        allowTransition = true;

        mapManager.FullActivation(this);
    }

    public void Deactivate()
    {
        isActivated = false;
        if (!allowTransition) mapManager.AddActive();
        allowTransition = true;
        
    }

    public void Setup(string titleText, string descText)
    {
        title.text = titleText;
        desc.text = descText;
    }
    #endregion
}
