using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    public Camera cam;

    [Header("Zoom settings")] 
    public float zoomModifierSpeed;
    public float minZoom;
    public float maxZoom;

    [Header("Pan Settings")]
    public float panSpeed;

    Vector3 touchPos;

    bool isInBound { get; set; }
    

    void LateUpdate()
    {

        if (!isInBound)
            return;

        CameraPanning();
        CameraZoom();
    }

    Vector3 GetWorldPos()
    {
        float dist;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane viewScreen = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        viewScreen.Raycast(ray, out dist);

        return ray.GetPoint(dist);
    }

    void CameraZoom()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0f)
        {
            cam.orthographicSize = Mathf.Clamp( cam.orthographicSize -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomModifierSpeed, minZoom, maxZoom);
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
            
            cam.orthographicSize = Mathf.Clamp( cam.orthographicSize -= difference * 0.01f, minZoom, maxZoom);
        }
    }

    void CameraPanning()
    {
        
        if (Input.touchCount >= 2) return;

        if (Input.GetMouseButtonDown(0)) touchPos = GetWorldPos();

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchPos - GetWorldPos();
            cam.transform.position = new Vector3(cam.transform.position.x + direction.x * panSpeed, cam.transform.position.y, cam.transform.position.z);
        }
    }

    public void IsInBoundary() => isInBound = true;
    public void IsNotInBound() => isInBound = false;

}
