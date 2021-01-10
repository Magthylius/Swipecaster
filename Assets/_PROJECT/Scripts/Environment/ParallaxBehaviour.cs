using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxBehaviour : MonoBehaviour
{
    EnvironmentManager ENV_Manager;

    [Header("Parallax Settings")]
    public ParallaxType type;

    public bool loop;

    [SerializeField] float multiplier;

    float startPos;
    float sprWidth;
    float temp;
    float verticalCamSize;
    float horizontalCamSize;
    float dist;
    Vector2 screenBound;

    GameObject camObj;
    Camera cam;
    SpriteRenderer sprRenderer;
    Bounds bound;

    [SerializeField] float leftBound, rightBound;
    
    
    void Start()
    {
        ENV_Manager = EnvironmentManager.instance;

        startPos = transform.position.x;
        sprRenderer = GetComponent<SpriteRenderer>();
        camObj = GameObject.FindWithTag("BattleCam");
        cam = camObj.GetComponent<Camera>();
        sprWidth = sprRenderer.sprite.bounds.size.x;
        bound = sprRenderer.bounds;
        
        verticalCamSize = cam.orthographicSize;
        horizontalCamSize = (verticalCamSize * cam.aspect);
        


    }

    void Update()
    {
        leftBound = (-bound.extents.x) + transform.position.x;
        rightBound = (bound.extents.x) + transform.position.x;
    }

    void LateUpdate()
    {
        if (loop)
        {
            temp = (camObj.transform.position.x * (1 - multiplier));
            //! How far the cam move in world space
            dist = (camObj.transform.position.x * multiplier);
            
            transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
            
            if (temp > startPos + sprWidth) startPos += sprWidth;
            else if (temp < startPos - sprWidth) startPos -= sprWidth;
        }
        else
        {
            
            //! How far the cam move in world space
            dist = (camObj.transform.position.x * multiplier);

            Vector3 curPos = transform.position;
            curPos.x = startPos - dist;
            
            if (leftBound > -horizontalCamSize + camObj.transform.position.x)
            {
                print("left");
            }
            else if (rightBound < horizontalCamSize + camObj.transform.position.x)
            {
                print("right");
            }
            else
                transform.position = curPos;


        }
    }
    

    #region Accessors

    public ParallaxType GetType() => type;

    public void SetMultiplier(float set) => multiplier = set;

    #endregion

}
