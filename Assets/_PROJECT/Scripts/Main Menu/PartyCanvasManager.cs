using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using LerpFunctions;
using UnityEngine.EventSystems;

public class PartyCanvasManager : MenuCanvasPage
{
    public static PartyCanvasManager instance;

    DatabaseManager database;
    
    [Header("Object References")]
    public CanvasGroup interactionGroup;
    public CanvasGroup configurationGroup;
    public CanvasGroup buttonGroup;
    public List<PartyGroupBehavior> partyList;
    public PartyConfigurationBehavior partyConfigurator;
    public RectTransform arrowSetter;

    [Header("Settings")]
    public float transitionSpeed = 5f;
    public float holdDownTimer = 1f;

    CanvasGroupFader interactionCGF;
    CanvasGroupFader configurationCGF;
    CanvasGroupFader buttonCGF;

    PartyGroupBehavior partyGroup;
    
    float timer = 0f;
    
    bool editMode = false;
    bool isHoldComplete = true;

    Vector3 arrowPosition;

    public override void Awake()
    {
        base.Awake();
        if (instance != null) Destroy(this);
        else instance = this;
    }

    void Start()
    {
        mainMenuManager = MainMenuManager.instance;
        database = DatabaseManager.instance;
        interactionCGF = new CanvasGroupFader(interactionGroup, true, true);
        configurationCGF = new CanvasGroupFader(configurationGroup, true, true);
        buttonCGF = new CanvasGroupFader(buttonGroup, true, true);
        arrowSetter.position = database.GetArrowTransform();
    }

    void Update()
    {
        interactionCGF.Step(transitionSpeed * Time.unscaledDeltaTime);
        configurationCGF.Step(transitionSpeed * Time.unscaledDeltaTime);
        buttonCGF.Step(transitionSpeed * Time.unscaledDeltaTime);
        HoldDownTimer();
    }
    
    void HoldDownTimer()
    {
        if (!isHoldComplete)
        {
            timer += Time.deltaTime;
            if (timer >= holdDownTimer)
            {
                BTN_EditParty(partyGroup);
                isHoldComplete = true;
            }
        }
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
    public void BTN_EditParty(PartyGroupBehavior _partyGroup)
    {
        
        partyConfigurator.SetConfigParty(partyGroup.party);
        partyConfigurator.UpdatePortraits();

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

    public void BTN_OnHoldDown(PartyGroupBehavior _partyGroup)
    {
        partyGroup = _partyGroup;
        isHoldComplete = false;
        timer = 0;
    }

    public void BTN_OnHoldUp(RectTransform shelf)
    {
        CasterParty party = shelf.GetComponent<PartyGroupBehavior>().party;
        arrowPosition = shelf.Find("Center").position;
        if (!isHoldComplete)
        {
            arrowSetter.position = arrowPosition;
            database.SetArrowTransform(arrowPosition);
            partyConfigurator.SetActiveParty(party);
        }

        isHoldComplete = true;
        timer = 0;
    }

    #endregion


}
