using System;
using LerpFunctions;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public Camera cam;

    [Header("Zoom settings")] public float zoomModifierSpeed;
    
    public float minZoom;
    public float maxZoom;
    [Header("Pan Settings")] public float panSpeed;
    
    public SpriteRenderer backgroundEnvSpr;
    
    float leftBound, rightBound, topBound, bottomBound;

    bool isInBound { get; set; }
    Vector3 touchPos;

    void Start()
    {
        UpdateCameraBoundary();
    }

    void LateUpdate()
    {
        if (!isInBound)
            return;

        UpdateCameraBoundary();
        
        CameraPanning();
        CameraZoom();
    }

    void CameraZoom()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0f)
        {
            float zoom = Input.GetAxisRaw("Mouse ScrollWheel") * zoomModifierSpeed;
            
            float targetMouseZoom = Mathf.Clamp(cam.orthographicSize - zoom, minZoom, maxZoom);
            
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetMouseZoom, zoomModifierSpeed * Time.unscaledDeltaTime);
            
            if (Lerp.NegligibleDistance(cam.orthographicSize, targetMouseZoom, 0.001f))
            {
                cam.orthographicSize = targetMouseZoom;
            }
            print("Editor Zoom");
        }


        if (Input.touchCount == 2)
        {
            Touch firstInput = Input.GetTouch(0);
            Touch secondInput = Input.GetTouch(1);

            Vector2 firstInputPrevPos = firstInput.position - firstInput.deltaPosition;
            Vector2 secondInputPrevPos = (secondInput.position - secondInput.deltaPosition);

            float prevMagnitude = (firstInputPrevPos - secondInputPrevPos).magnitude;
            float currentMagnitude = (firstInput.position - secondInput.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            
            float targetZoom = Mathf.Clamp(cam.orthographicSize - difference * 0.01f, minZoom, maxZoom);
            
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomModifierSpeed * Time.unscaledDeltaTime);

            if (Lerp.NegligibleDistance(cam.orthographicSize, targetZoom))
            {
                cam.orthographicSize = targetZoom;
            }
            print("Phone Zoom");
        }
    }

    void CameraPanning()
    {
        if (Input.touchCount >= 2) return;

        if (Input.GetMouseButtonDown(0)) touchPos = GetWorldPos();

        if (Input.GetMouseButton(0))
        {
         
            Vector3 direction = touchPos - GetWorldPos();
            cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x + direction.x, leftBound, rightBound) * panSpeed,
                cam.transform.position.y, cam.transform.position.z);
        }
    }

    void UpdateCameraBoundary()
    {
        float verticalCamSize = cam.orthographicSize;
        float horizontalCamSize = (verticalCamSize * cam.aspect);

        Bounds levelBounds = backgroundEnvSpr.bounds;
        
        leftBound = (levelBounds.min.x) + horizontalCamSize;
        rightBound = (levelBounds.max.x) - horizontalCamSize;
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
}