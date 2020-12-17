using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraFunctions;

public class RuneBehaviour : MonoBehaviour
{
    RuneManager runeManager;
    ConnectionManager connectionManager;

    Rigidbody2D rb;
    Transform _transform;

    float maxVelocity;
    GameObject self;
    bool selected = false;
    Vector2 position;

    bool allowMouse = false;

    public RuneType type;
    Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        cam = Camera.main;
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
        position = _transform.position;
        SelfDeactivate();
    }
    
    void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public void SelfDeactivate()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(position);

        if (viewPos.y < 0)
        {
            runeManager.GetActiveRuneList().Remove(this.gameObject);
            rb.velocity = new Vector2(0, -10);
            gameObject.SetActive(false);
     
            if (selected) connectionManager.Disconnect(this);

            selected = false;
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
    public Vector2 GetPosition() => position;

    public bool GetSelected() => selected;

    public bool SetSelected(bool set) => selected = false;

    #endregion

}
