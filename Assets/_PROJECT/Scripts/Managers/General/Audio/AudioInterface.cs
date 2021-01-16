using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInterface : MonoBehaviour
{
    AudioManager audioManager;
    
    public AudioData audioPack;

    void Start() => audioManager = AudioManager.instance;

    public void PlayTap() => audioManager.PlaySFX(audioPack, "Tap");

    public void PlayClick() => audioManager.PlaySFX(audioPack, "Click");

    public void PlayWhoose() => audioManager.PlaySFX(audioPack, "Whoosh");

}
