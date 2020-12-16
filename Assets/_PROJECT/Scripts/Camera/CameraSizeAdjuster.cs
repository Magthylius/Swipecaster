using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeAdjuster : MonoBehaviour
{
    [Header("Background Environment Sprite")]
    public SpriteRenderer backgroundEnv;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Start ()
    {
        float screenSize = backgroundEnv.bounds.size.y * Screen.height / Screen.width * 0.5f;
        cam.orthographicSize = screenSize;

    }
}
