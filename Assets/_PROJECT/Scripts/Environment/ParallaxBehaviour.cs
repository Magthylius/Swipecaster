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

    [SerializeField] float multiplier;
    
    float sprWidth;
    float startPos;
    [SerializeField] float temp;
    GameObject camTransform;

    void Start()
    {
        ENV_Manager = EnvironmentManager.instance;

        startPos = transform.position.x;
        sprWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        camTransform = GameObject.FindWithTag("BattleCam");
    }

    void LateUpdate()
    {
        temp = (camTransform.transform.position.x * (1 - multiplier));
        //! How far the cam move in world space
        float dist = (camTransform.transform.position.x * multiplier);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + sprWidth) startPos += sprWidth;
        else if (temp < startPos - sprWidth) startPos -= sprWidth;
    }

    #region Accessors

    public ParallaxType GetType() => type;

    public void SetMultiplier(float set) => multiplier = set;

    #endregion

}
