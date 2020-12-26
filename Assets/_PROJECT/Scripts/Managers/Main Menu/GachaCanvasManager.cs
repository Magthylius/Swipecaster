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

public class GachaCanvasManager : MonoBehaviour
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

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        connectedList = new List<GachaConnectorBehavior>();
        linePoints = new List<Vector2>();

        chargeImg.fillAmount = 0f;
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

        }
    }

    public void ConnectGachaPoint(GachaPoint point)
    {
        if (CheckEligibleConnector(point))
        {
            connectedList.Add(FindConnector(point));
            linePoints.Add(FindConnector(point).center);
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

    bool CheckEligibleConnector(GachaPoint point)
    {
        foreach (GachaConnectorBehavior connector in connectedList)
        {
            if (connector.type == point) return false;
        }

        return true;
    }
    public bool GetAllowCasting() => allowCasting;
    #endregion
}
