using System;
using System.Collections;
using LerpFunctions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Camera cam;

    [Header("Edge offset values")] 
    public float horizontalOffset;
    public float verticalOffset;

    [Header("Zoom settings")] 
    public float zoomModifierSpeed;
    public float minZoom;
    public float maxZoom;

    [Header("Pan Settings")] public float panSpeed;

    [Header("Unit Zoom")] float unitZoomAnimation = 2;

    public SpriteRenderer backgroundEnvSpr;

    float leftBound, rightBound, topBound, bottomBound;

    float targetZoom = 0f;
    float prevZoom;
    bool allowZoom = false;

    float targetPan = 0f;
    float prevPan;
    bool allowPan = false;

    bool isInBound { get; set; }
    bool isFree { get; set; }

    Vector3 touchPos;
    Transform targetUnit;

    #region Debug Zoom Animation

    [Header("Debug Zoom Animation")] public float countdown;

    #endregion

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        UpdateCameraBoundary();
        targetZoom = cam.orthographicSize;
        cam.transform.position = new Vector3(cam.transform.position.x, bottomBound, cam.transform.position.z);
        isFree = true;
    }

    void LateUpdate()
    {
        if (allowZoom)
        {
            cam.orthographicSize =
                Mathf.Lerp(cam.orthographicSize, targetZoom, zoomModifierSpeed * Time.unscaledDeltaTime);

            if (Lerp.NegligibleDistance(cam.orthographicSize, targetZoom, 0.001f))
            {
                cam.orthographicSize = targetZoom;
                allowZoom = false;
            }
        }

        if (allowPan)
        {
            float actualPan = Mathf.Lerp(cam.transform.position.x, targetPan, panSpeed * Time.unscaledDeltaTime);
            cam.transform.position = new Vector3(actualPan, cam.transform.position.y, cam.transform.position.z);

            if (Lerp.NegligibleDistance(cam.orthographicSize, targetPan, 0.001f))
            {
                cam.transform.position = new Vector3(targetPan, cam.transform.position.y, cam.transform.position.z);
                allowPan = false;
            }
        }

        UpdateCameraBoundary();

        if (!isInBound || !isFree)
            return;

        CameraZoom();
        CameraPanning();
    }

    void CameraZoom()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0f)
        {
            allowZoom = true;
            float zoom = Input.GetAxisRaw("Mouse ScrollWheel");

            targetZoom = Mathf.Clamp(cam.orthographicSize - zoom, minZoom, maxZoom);
        }


        if (Input.touchCount == 2)
        {
            Touch firstInput = Input.GetTouch(0);
            Touch secondInput = Input.GetTouch(1);

            Vector2 firstInputPrevPos = firstInput.position - firstInput.deltaPosition;
            Vector2 secondInputPrevPos = secondInput.position - secondInput.deltaPosition;

            float prevMagnitude = (firstInputPrevPos - secondInputPrevPos).magnitude;
            float currentMagnitude = (firstInput.position - secondInput.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            if (difference != 0)
            {
                allowZoom = true;
                targetZoom = Mathf.Clamp(cam.orthographicSize - difference * 0.01f, minZoom, maxZoom);
            }
        }
    }

    void CameraPanning()
    {
        if (Input.touchCount >= 2) return;

        if (Input.GetMouseButtonDown(0))
        {
            touchPos = GetWorldPos();
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchPos - GetWorldPos();
            if (direction.sqrMagnitude >= 1) allowPan = true;

            targetPan = Mathf.Clamp(cam.transform.position.x + direction.x, leftBound, rightBound);
        }
    }

    void UpdateCameraBoundary()
    {
        float verticalCamSize = cam.orthographicSize;
        float horizontalCamSize = (verticalCamSize * cam.aspect);

        Bounds levelBounds = backgroundEnvSpr.bounds;

        leftBound = (levelBounds.min.x) + (horizontalCamSize + horizontalOffset);
        rightBound = (levelBounds.max.x) - (horizontalCamSize + horizontalOffset);
        bottomBound = (levelBounds.min.y) + (verticalCamSize + verticalOffset);

        cam.transform.position = new Vector3(cam.transform.position.x, bottomBound, cam.transform.position.z);
    }

    public void ZoomToUnit(GameObject unit)
    {
        //! Get Target Unit
        targetUnit = unit.transform;

        //! 
        prevZoom = targetZoom;
        prevPan = targetPan;

        isFree = false;
        targetZoom = unitZoomAnimation;
        targetPan = unit.transform.position.x;
        allowPan = true;
        allowZoom = true;
        StartCoroutine(ZoomInTimer());
    }

    public void MoveToUnit(GameObject unit)
    {
        targetPan = unit.transform.position.x;
        allowPan = true;
    }
    
    

    public void IsInBoundary() => isInBound = true;
    public void IsNotInBound() => isInBound = false;

    Vector3 GetWorldPos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane viewScreen = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        viewScreen.Raycast(ray, out var dist);

        return ray.GetPoint(dist);
    }

    IEnumerator ZoomInTimer()
    {
        float timer = 0;

        while (timer <= countdown)
        {
            print(timer);
            timer += Time.deltaTime;

            yield return null;
        }


        targetPan = prevPan;
        targetZoom = prevZoom;
        allowPan = true;
        allowZoom = true;

        isFree = true;
    }

    #region Accessors

    public void SetIsFree(bool _isFree) => isFree = _isFree;
    public bool GetIsFree() => isFree;

    #endregion
}