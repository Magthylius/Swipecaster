using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GachaPoint
{
    CENTER = 0,
    TOP,
    TOP_RIGHT,
    RIGHT,
    BOT_RIGHT,
    BOT,
    BOT_LEFT,
    LEFT,
    TOP_LEFT
}

public class GachaCanvasManager : MenuCanvasPage
{
    enum GachaCanvasState
    {
        IDLE = 0,
        CHARGING,
        DISCHARGING,
        SUMMONING
    }

    public static GachaCanvasManager instance;
    GachaCanvasState state = GachaCanvasState.IDLE;

    DatabaseManager dataManager;
    InventoryManager invManager;

    [Header("Connector Points")]
    public UILineRenderer uiLine;
    public List<GachaConnectorBehavior> connectorList;

    List<GachaConnectorBehavior> connectedList;
    List<Vector2> linePoints;

    bool allowCasting = true;

    [Header("Cover")]
    public Image chargeImg;
    public Image coverImg;
    public float chargeSpeed = 1f;
   
    float charge;
    float chargePrecision = 0.001f;

    [Header("Accessories")]
    public CanvasGroup summonButton;
    public TextMeshProUGUI instructionText;

    [Header("Gacha Settings")]
    public GachaBanner activeBanner;

    [Header("Tutorial Condition")]
    public bool isTutorialed;

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

        connectedList = new List<GachaConnectorBehavior>();
        linePoints = new List<Vector2>();

        chargeImg.fillAmount = 0f;

        summonButton.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (state != GachaCanvasState.IDLE && state != GachaCanvasState.SUMMONING)
        {
            if (state == GachaCanvasState.CHARGING)
            {
                charge = Mathf.Lerp(charge, 1f, chargeSpeed * Time.unscaledDeltaTime);

                if (1f - charge <= chargePrecision)
                {
                    state = GachaCanvasState.IDLE;
                    charge = 1f;
                }
            }
            else if (state == GachaCanvasState.DISCHARGING)
            {
                charge = Mathf.Lerp(charge, 0f, chargeSpeed * Time.unscaledDeltaTime);

                if (charge <= chargePrecision)
                {
                    state = GachaCanvasState.IDLE;
                    charge = 0f;
                }
            }

            chargeImg.fillAmount = charge;
            if (charge >= 1f)
            {
                chargeImg.gameObject.SetActive(false);
                state = GachaCanvasState.SUMMONING;
            }
        }
        else if (state == GachaCanvasState.SUMMONING)
        {
            summonButton.gameObject.SetActive(true);
            instructionText.gameObject.SetActive(false);
        }
    }
    
    public override void Reset()
    {
        charge = 0f;
        chargeImg.gameObject.SetActive(true);
        chargeImg.fillAmount = charge;

        connectedList = new List<GachaConnectorBehavior>();
        linePoints = new List<Vector2>();
        uiLine.UpdatePoints(linePoints);

        summonButton.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(true);
        state = GachaCanvasState.IDLE;
    }

    public void ConnectGachaPoint(GachaPoint point)
    {
        if (!CheckConnectorConnected(point))
        {
            connectedList.Add(FindConnector(point));
            linePoints.Add(FindConnector(point).center);
            uiLine.UpdatePoints(linePoints);
        }
    }

    public void DisconnectGachaPoint(GachaPoint point)
    {
        if (CheckConnectorConnected(point))
        {
            connectedList.Remove(FindConnector(point));
            linePoints.Remove(FindConnector(point).center);
            uiLine.UpdatePoints(linePoints);
        }
    }

    public void CoverCharge()
    {
        state = GachaCanvasState.CHARGING;
    }

    public void CoverDischarge()
    {
        state = GachaCanvasState.DISCHARGING;
    }

    public void BTN_SummonCharacter()
    {
        if (!isTutorialed)
        {
            DatabaseManager.instance.SaveTutorialState(TutorialPhase.guideToParty);
            //DialogueManager.instance.tutorialPhase = TutorialPhase.guideToParty;
            DialogueManager.instance.guideToParty();
            isTutorialed = true;
        }

        UnitObject pull = activeBanner.PullCaster();
        print(pull.CharacterName);
        dataManager.AddCaster(pull.ID);
        invManager.UpdateCasterInventory();
    }

    #region Accessors
    GachaConnectorBehavior FindConnector(GachaPoint point)
    {
        foreach (GachaConnectorBehavior connector in connectorList)
        {
            if (point == connector.type) return connector;
        }

        Debug.LogError("GachaConnector not found!");
        return null;
    }

    bool CheckConnectorConnected(GachaPoint point)
    {
        foreach (GachaConnectorBehavior connector in connectedList)
        {
            if (connector.type == point) return true;
        }

        return false;
    }
    public bool GetAllowCasting() => allowCasting;
    #endregion
}
