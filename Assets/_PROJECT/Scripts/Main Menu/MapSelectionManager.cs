using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LerpFunctions;
using UnityEngine.UI;

public class MapSelectionManager : MenuCanvasPage
{
    public static MapSelectionManager instance;

    public QuestHandler questHandler;
    public CanvasGroup levelSelector;
    public GameObject levelSelectDisabler;
    public float levelSelectorSpeed = 3f;
    public LayoutGroup layoutGroup;

    List<LevelSelectBehavior> selectionList;
    bool layoutActive = false;
    int activeCount = 0;

    CanvasGroupFader levelSelectCGF;
    bool showLevelSelect = false;


    public override void Awake()
    {
        base.Awake();
        if (instance != null) Destroy(this);
        else instance = this;
    }

    void Start()
    {
        levelSelectCGF = new CanvasGroupFader(levelSelector, true, true);
        selectionList = questHandler.levelSelectors;
    }

    void Update()
    {
        if (layoutActive) layoutGroup.SetLayoutVertical();
        levelSelectCGF.Step(levelSelectorSpeed * Time.unscaledDeltaTime);
    }

    public override void Reset()
    {
        BTN_HideLevelSelect();
    }

    public void FullActivation(LevelSelectBehavior self = null)
    {
        if (self == null)
        {
            foreach (LevelSelectBehavior selects in selectionList) selects.Deactivate();
            return;
        }

        foreach (LevelSelectBehavior selects in selectionList)
        {
            if (selects != self && selects.gameObject.activeInHierarchy) selects.Deactivate();
        }
    }

    void CheckAllowActive()
    {
        if (activeCount > 0)
            layoutActive = true;
        else
            layoutActive = false;
    }

    public void AddActive()
    {
        activeCount++;
        CheckAllowActive();
    }

    public void RemoveActive()
    {
        activeCount--;
        CheckAllowActive();
    }

    public void BTN_ShowLevelSelect()
    {
        showLevelSelect = true;
        levelSelectDisabler.SetActive(true);
        levelSelectCGF.StartFadeIn();
    }

    public void BTN_HideLevelSelect()
    {
        levelSelectDisabler.SetActive(false);
        showLevelSelect = false;
        levelSelectCGF.StartFadeOut();
        FullActivation(null);
    }
}
