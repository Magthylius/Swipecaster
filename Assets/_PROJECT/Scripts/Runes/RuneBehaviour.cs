using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraFunctions;
using UnityEngine.UI;

public class RuneBehaviour : MonoBehaviour
{
    RuneManager runeManager;
    ConnectionManager connectionManager;
    SettingsManager settingsManager;
    
    public RuneType type;
    public Sprite activatedSprite;
    public Sprite deactivatedSprite;
    public bool imageMode = false;

    Rigidbody2D rb;
    Transform _transform;
    RectTransform rt;
    GameObject self;
    SpriteRenderer sr;
    Image img;

    Vector2 positionInScreen;
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
        rt = GetComponent<RectTransform>();
        cam = Camera.main;
        self = gameObject;
        sr = GetComponent<SpriteRenderer>();
        img = GetComponent<Image>();

        if (imageMode) sr.enabled = false;
        else img.enabled = false;
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

        Deactivate();
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

                if (imageMode) img.sprite = deactivatedSprite;
                else sr.sprite = deactivatedSprite;
            }
            else if (allowMouse && Input.GetMouseButtonUp(0))
            {
                selected = false;

                if (imageMode) img.sprite = deactivatedSprite;
                else sr.sprite = deactivatedSprite;
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
        if (imageMode) img.sprite = deactivatedSprite;
        else sr.sprite = deactivatedSprite;
    }

    public void SelfDeactivate()
    {
        if (isPaused) return;

        if (!imageMode && !sr.isVisible)
        {
            Deactivate();
        }
        else if (imageMode && Cam.IsVisibleFrom(rt, cam))
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
            //print("selection!");
            if (imageMode) img.sprite = activatedSprite;
            else sr.sprite = activatedSprite;

            connectionManager.StartSelection(this);
        }
        else if (connectionManager.GetSelectionType() == type)
        {
            selected = true;

            if (imageMode) img.sprite = activatedSprite;
            else sr.sprite = activatedSprite;

            connectionManager.Connect(this);
        }
    }

    public void ResetToActivateSprite()
    {
        if (isPaused) return;
        selected = false;

        if (imageMode) img.sprite = deactivatedSprite;
        else sr.sprite = deactivatedSprite;
    }

    #region Queries
    public RuneType GetRuneType() => type;
    public GameObject GetSelf() => self;
    public Vector2 GetPosition() => transform.position;

    public bool GetSelected() => selected;

    public bool SetSelected(bool set) => selected = set;

    #endregion

}
