using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    AudioManager AM;

    void Start()
    {
        AM = AudioManager.instance;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)) AM.PlaySFX(AudioType.SFX_FART);
    }
}
