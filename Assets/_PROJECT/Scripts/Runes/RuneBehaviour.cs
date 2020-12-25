using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraFunctions;

public class RuneBehaviour : MonoBehaviour
{
    RuneManager runeManager;
    ConnectionManager connectionManager;
    SettingsManager settingsManager;
    
    public RuneType type;
    public Sprite activatedSprite;
    public Sprite deactivatedSprite;

    Rigidbody2D rb;
    Transform _transform;
    GameObject self;
    Vector2 positionInScreen;
    SpriteRenderer sr;
    float spriteHeight;

    float maxVelocity;
    bool selected = false;
    bool allowMouse = false;

    Camera cam;
    bool isPaused = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        cam = Camera.main;
        self = gameObject;
        sr = GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {
        runeManager = RuneManager.instance;
        connectionManager = ConnectionManager.instance;
        settingsManager = SettingsManager.instance;

        maxVelocity = runeManager.maxVelocity;
        allowMouse = runeManager.allowMouse;

        settingsManager.pausedEvent.AddListener(SetStatePaused);
        settingsManager.unpausedEvent.AddListener(SetStateUnpaused);
    }
    
    void Update()
    {
        if (isPaused) return;

        SelfDeactivate();

        if (selected) 
        {
            if (!allowMouse && Input.touchCount < 1)
            {
                selected = false;
                sr.sprite = deactivatedSprite;
            }
            else if (allowMouse && Input.GetMouseButtonUp(0))
            {
                selected = false;
                sr.sprite = deactivatedSprite;
            }
        }
    }
    
    void FixedUpdate()
    {
        if (isPaused) return;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    void SetStatePaused() => isPaused = true;
    void SetStateUnpaused() => isPaused = false;

    public void Deactivate()
    {
        if (isPaused) return;
        runeManager.GetActiveRuneList().Remove(this.gameObject);
        rb.velocity = new Vector2(0, -10);
        gameObject.SetActive(false);

        if (selected) connectionManager.Disconnect(this);

        selected = false;
        sr.sprite = deactivatedSprite;
    }

    public void SelfDeactivate()
    {
        if (isPaused) return;
        if (!sr.isVisible)
        {
            Deactivate();
        }
    }
    

    public void Selected()
    {
        if (isPaused) return;
        if (allowMouse && !Input.GetMouseButton(0)) return;

        if (!connectionManager.GetSelectionStart())
        {
            selected = true;
            sr.sprite = activatedSprite;
            connectionManager.StartSelection(this);
        }
        else if (connectionManager.GetSelectionType() == type)
        {
            selected = true;
            sr.sprite = activatedSprite;
            connectionManager.Connect(this);
        }
    }

    public void ResetToActivateSprite()
    {
        if (isPaused) return;
        selected = false;
        sr.sprite = deactivatedSprite;
    }

    #region Queries
    public RuneType GetRuneType() => type;
    public GameObject GetSelf() => self;
    public Vector2 GetPosition() => transform.position;

    public bool GetSelected() => selected;

    public bool SetSelected(bool set) => selected = set;

    #endregion

}
