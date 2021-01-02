using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LerpFunctions;

public class PartyCanvasManager : MenuCanvasPage
{
    public static PartyCanvasManager instance;

    [Header("Object References")]
    public CanvasGroup interactionGroup;
    public CanvasGroup configurationGroup;
    public CanvasGroup buttonGroup;
    public List<PartyGroupBehavior> partyList;
    public PartyConfigurationBehavior partyConfigurator;

    [Header("Settings")]
    public float transitionSpeed = 5f;

    CanvasGroupFader interactionCGF;
    CanvasGroupFader configurationCGF;
    CanvasGroupFader buttonCGF;

    bool editMode = false;

    public override void Awake()
    {
        base.Awake();
        if (instance != null) Destroy(this);
        else instance = this;
    }

    void Start()
    {
        mainMenuManager = MainMenuManager.instance;
        interactionCGF = new CanvasGroupFader(interactionGroup, true, true);
        configurationCGF = new CanvasGroupFader(configurationGroup, true, true);
        buttonCGF = new CanvasGroupFader(buttonGroup, true, true);
    }

    void Update()
    {
        interactionCGF.Step(transitionSpeed * Time.unscaledDeltaTime);
        configurationCGF.Step(transitionSpeed * Time.unscaledDeltaTime);
        buttonCGF.Step(transitionSpeed * Time.unscaledDeltaTime);
    }

    void EditMode()
    {
        editMode = true;

        interactionCGF.StartFadeOut();
        configurationCGF.StartFadeIn();
        buttonCGF.StartFadeIn();

        mainMenuManager.HideBottomOverlay();
    }

    void PartyMode()
    {
        editMode = false;

        interactionCGF.StartFadeIn();
        configurationCGF.StartFadeOut();
        buttonCGF.StartFadeOut();

        mainMenuManager.ShowBottomOverlay();
    }

    #region Buttons
    public void BTN_EditParty(PartyGroupBehavior partyGroup)
    {
        partyConfigurator.SetConfigParty(partyGroup.party);
        partyConfigurator.UpdatePortraits();
        partyConfigurator.UpdateCasters();

        EditMode();
    }

    public void BTN_ApplyEdit()
    {
        PartyMode();
    }

    public void BTN_CancelEdit()
    {
        PartyMode();
    }
    #endregion
}
