using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LerpFunctions;

public class HomeCanvasManager : MenuCanvasPage
{
    public static HomeCanvasManager instance;
    DatabaseManager dataManager;
    InventoryManager invManager;


    public CanvasGroup resetCanvasGroup;
    public float transitionSpeed = 5f;
    public PartyGroupBehavior[] partyConfig;
    
    CanvasGroupFader resetCGF;

    public override void Awake()
    {
        base.Awake();
        if (instance != null) Destroy(this);
        else instance = this; 
    }

    void Start()
    {
        dataManager = DatabaseManager.instance;
        invManager = InventoryManager.instance;

        resetCGF = new CanvasGroupFader(resetCanvasGroup, true, true);
        resetCGF.SetTransparent();
    }

    void Update()
    {
        resetCGF.Step(transitionSpeed * Time.unscaledDeltaTime);
    }

    public override void Reset()
    {
        resetCGF.SetTransparent();
        resetCGF.SetStateFadeIn();
    }

    public void BTN_ShowResetCanvas()
    {
        resetCGF.StartFadeIn();
    }

    public void BTN_HideResetCanvas(bool resetData)
    {
        if (resetData)
        {
            dataManager.GenerateNewSaveData();
            invManager.UpdateCasterInventory();
            foreach (PartyGroupBehavior _partyGroup in partyConfig)
            {
                _partyGroup.UpdateAll();
            }

            CurrencyManager.instance.UpdateTexts();
        }

        resetCGF.StartFadeOut();
    }
}
