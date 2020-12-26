using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /*[System.Serializable]
    public struct GachaConnectors
    {
        public RectTransform connector;
        public GachaPoint point;

        Vector2 connectorCenter;
        public void CalculateCenter() => connectorCenter = (connector.offsetMax + connector.offsetMin) * 0.5f;
        public Vector2 GetCenter() => connectorCenter;
    }*/

    public static GachaCanvasManager instance;

    public UILineRenderer uiLine;
    public List<GachaConnectorBehavior> connectorList;

    List<GachaConnectorBehavior> connectedList;
    List<Vector2> linePoints;

    bool allowCasting = true;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        //foreach (GachaConnectors connector in connectorList) connector.CalculateCenter();
        connectedList = new List<GachaConnectorBehavior>();

        linePoints = new List<Vector2>();
        //linePoints.Add(Vector2.zero);
    }

    void FixedUpdate()
    {
        /*if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            linePoints[linePoints.Count - 1] = Input.mousePosition;
            uiLine.UpdatePoints(linePoints);
        }*/
    }

    public void ConnectGachaPoint(GachaPoint point)
    {
        if (CheckEligibleConnector(point))
        {
            connectedList.Add(FindConnector(point));
            linePoints.Add(FindConnector(point).center);
            //linePoints.Add(Vector2.zero);
            uiLine.UpdatePoints(linePoints);
        }
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
