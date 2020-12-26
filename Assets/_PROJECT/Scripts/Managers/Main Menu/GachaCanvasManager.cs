using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaCanvasManager : MonoBehaviour
{
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

    [System.Serializable]
    public struct GachaConnectors
    {
        public RectTransform connector;
        public GachaPoint point;

        Vector2 connectorCenter;
        public void CalculateCenter() => connectorCenter = (connector.offsetMax + connector.offsetMin) * 0.5f;
        public Vector2 GetCenter() => connectorCenter;
    }

    public UILineRenderer uiLine;
    public List<GachaConnectors> connectorList;

    List<GachaConnectors> connectedList;
    List<Vector2> linePoints;
    
    void Start()
    {
        foreach (GachaConnectors connector in connectorList) connector.CalculateCenter();
        connectedList = new List<GachaConnectors>();

        linePoints = new List<Vector2>();
        linePoints.Add(Vector2.zero);
    }

    void FixedUpdate()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            linePoints[linePoints.Count - 1] = Input.mousePosition;
            uiLine.UpdatePoints(linePoints);
        }
    }

    public void ConnectGachaPoint(GachaPoint point)
    {
        if (CheckEligibleConnector(point))
        {
            connectedList.Add(FindConnector(point));
            linePoints[linePoints.Count - 1] = FindConnector(point).GetCenter();
            linePoints.Add(Vector2.zero);
        }
    }

    #region Accessors
    GachaConnectors FindConnector(GachaPoint point)
    {
        foreach (GachaConnectors connector in connectorList)
        {
            if (point == connector.point) return connector;
        }

        Debug.LogError("GachaConnector not found!");
        return new GachaConnectors();
    }

    bool CheckEligibleConnector(GachaPoint point)
    {
        foreach (GachaConnectors connector in connectedList)
        {
            if (connector.point == point) return false;
        }

        return true;
    }
    #endregion
}
