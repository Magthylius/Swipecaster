using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaConnectorBehavior : MonoBehaviour
{
    GachaCanvasManager gachaManager;

    public GachaPoint pointType;

    bool isActivated = false;
    Vector2 selfCenter;

    void Start()
    {
        gachaManager = GachaCanvasManager.instance;
        RectTransform rt = GetComponent<RectTransform>();
        selfCenter = (rt.offsetMax + rt.offsetMin) * 0.5f;
    }

    public void PNTR_Enter()
    {
        if (!gachaManager.GetAllowCasting()) return;
        if (isActivated) return;

        gachaManager.ConnectGachaPoint(pointType);
        isActivated = true;
    }

    public void SetActviation(bool activation)
    {
        isActivated = activation;
    }

    #region accessors
    public Vector2 center => selfCenter;
    public GachaPoint type => pointType;
    #endregion
}
