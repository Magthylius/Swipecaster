using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBehaviour : MonoBehaviour
{
    EnvironmentManager ENV_Manager;

    [Header("Parallax Settings")]
    public ParallaxType type;
    public bool infiniteHorizontal;
    public bool infiniteVertical;
    
    Transform camTransform;
    Vector3 lastCameraPos;
    public Vector2 multiplier;

    float spriteSizeX, spriteSizeY;
    
    void Start()
    {
        ENV_Manager = EnvironmentManager.instance;
        
        camTransform = GameObject.FindWithTag("BattleCam").GetComponent<Transform>();
        lastCameraPos = camTransform.position;
        Sprite spr = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = spr.texture;
        spriteSizeX = (texture.width / spr.pixelsPerUnit) * transform.localScale.x;
        spriteSizeY = (texture.height / spr.pixelsPerUnit) * transform.localScale.y;
    }

    void LateUpdate()
    {
        Vector3 deltaPos = camTransform.position - lastCameraPos;
        transform.position += new Vector3(deltaPos.x * multiplier.x, deltaPos.y * multiplier.y);
        lastCameraPos = camTransform.position;

        if (infiniteHorizontal)
        {
            if (Mathf.Abs(camTransform.position.x - transform.position.x) >= spriteSizeX) 
            {
                float offsetPositionX = (camTransform.position.x - transform.position.x) % spriteSizeX;
                transform.position = new Vector3(camTransform.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (infiniteVertical)
        {
            if (Mathf.Abs(camTransform.position.x - transform.position.x) >= spriteSizeX) 
            {
                float offsetPositionY = (camTransform.position.y - transform.position.y) % spriteSizeY;
                transform.position = new Vector3(transform.position.x, camTransform.position.y + offsetPositionY);
            }
        }
        
    }

    #region Accessors

    public ParallaxType GetType() => type;

    public Vector2 SetMultiplier(Vector2 set) => multiplier = set;

    #endregion

}
