using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraFunctions;

public class RuneBehaviour : MonoBehaviour
{
    RuneManager runeManager;
    ConnectionManager connectionManager;

    Rigidbody2D rb;
    RectTransform rt;

    float maxVelocity;
    GameObject self;
    bool selected = false;

    bool allowMouse = false;

    public RuneType type;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rt = GetComponent<RectTransform>();
        self = gameObject;
    }
    
    void Start()
    {
        runeManager = RuneManager.instance;
        connectionManager = ConnectionManager.instance;

        maxVelocity = runeManager.maxVelocity;
        allowMouse = runeManager.allowMouse;
    }
    
    void Update()
    {
        SelfDeactivate();
    }
    
    void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public void SelfDeactivate()
    {
        if (!Cam.IsVisibleFrom(rt, Camera.main))
        {
            runeManager.GetActiveRuneList().Remove(this.gameObject);
            rb.velocity = new Vector2(0, -10);
            gameObject.SetActive(false);

            if (selected) connectionManager.Disconnect(this);
        }
    }

    public void Selected()
    {
        if (allowMouse && !Input.GetMouseButton(0)) return;

        selected = true;

        if (!connectionManager.GetSelectionStart())
            connectionManager.StartSelection(this);
        else if (connectionManager.GetSelectionType() == type)
        {
            //print("connect");
            connectionManager.Connect(this);
        }
    }

    #region Queries
    public RuneType GetRuneType() => type;
    public GameObject GetSelf() => self;
    #endregion

}
