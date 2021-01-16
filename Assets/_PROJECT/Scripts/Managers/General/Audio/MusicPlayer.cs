using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioManager audioManager;

    public AudioData audioPack;
    
    void Start()
    {
        audioManager = AudioManager.instance;
        
        audioManager.PlayBGM(audioPack, "BGM");
    }
}
