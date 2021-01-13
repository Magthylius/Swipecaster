using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    AudioManager AM;

    public AudioData audioSo;
    
    void Start()
    {
        AM = AudioManager.instance;
    }

    void Update()
    {
     
        if(Input.GetKeyDown(KeyCode.S)) AM.PlayRandomSFX(audioSo, "FartPack");
    }
}
