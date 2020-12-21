using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera cam;
    public float zoomSpeed;
    public float panSpeed;
    Vector3 touchPos;
    float curZoom;

    void LateUpdate()
    {
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
            cam.fieldOfView = Mathf.Clamp( cam.fieldOfView -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed, 10, 20);
        }
        
        
        
    }
    
    void CameraPanning()
    {
        if (Input.GetMouseButtonDown(0)) touchPos = GetWorldPos();

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchPos - GetWorldPos();
            cam.transform.position = new Vector3(cam.transform.position.x + direction.x, cam.transform.position.y, cam.transform.position.z);
        }
    }
    
}
