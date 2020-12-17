using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraFunctions;

public class RuneBehaviour : MonoBehaviour
{
    RuneManager runeManager;
    ConnectionManager connectionManager;
    
    public RuneType type;

    float maxVelocity;
    bool selected = false;
    bool allowMouse = false;
    
    Rigidbody2D rb;
    Transform _transform;
    GameObject self;
    Vector2 exitPos;
    SpriteRenderer sr;
    float spriteHeight;

    Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        cam = Camera.main;
        self = gameObject;
        sr = GetComponent<SpriteRenderer>();
        spriteHeight = sr.sprite.bounds.size.y / 2;

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
        exitPos = new Vector2(_transform.position.x, _transform.position.y + spriteHeight);
        SelfDeactivate();
    }
    
    void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public void Deactivate()
    {
        runeManager.GetActiveRuneList().Remove(this.gameObject);
        rb.velocity = new Vector2(0, -10);
        gameObject.SetActive(false);

        if (selected) connectionManager.Disconnect(this);

        selected = false;
    }

    public void SelfDeactivate()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(exitPos);

        if (viewPos.y < 0)
        {
            Deactivate();
        }
    }

    public void Selected()
    {
        if (allowMouse && !Input.GetMouseButton(0)) return;
        

        if (!connectionManager.GetSelectionStart())
        {
            selected = true;
            connectionManager.StartSelection(this);
        }
        else if (connectionManager.GetSelectionType() == type)
        {
            //print("connect");
            selected = true;
            connectionManager.Connect(this);
        }
    }

    #region Queries
    public RuneType GetRuneType() => type;
    public GameObject GetSelf() => self;
    public Vector2 GetPosition() => transform.position;

    public bool GetSelected() => selected;

    public bool SetSelected(bool set) => selected = set;

    #endregion

}
