using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundFile
{
    public AudioClip clip;
    public AudioType type;
    [Range(0.0f,1.0f)]
    public float volume;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource SFX_Source, BGM_Source;

    public List<SoundFile> soundList = new List<SoundFile>();

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    SoundFile GetSound(AudioType _audioType)
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            if(soundList[i].type == _audioType)
            {
                return soundList[i];
            }
        }

        return null;
    }

    public void PlaySFX(AudioType _audioType)
    {
        SoundFile sound = GetSound(_audioType);
        if(sound != null)
        {
            SFX_Source.volume = sound.volume;
            SFX_Source.PlayOneShot(sound.clip);
        }
    }
    public void PlayLoopingSFX(AudioType _audioType)
    {
        SoundFile sound = GetSound(_audioType);
        if (sound != null)
        {
            SFX_Source.volume = sound.volume;
            SFX_Source.clip = sound.clip;
            SFX_Source.loop = true;

            SFX_Source.Play();
        }
    }
    public void PlayBGM(AudioType _audioType)
    {
        SoundFile sound = GetSound(_audioType);
        if (sound != null)
        {
            BGM_Source.volume = sound.volume;
            BGM_Source.clip = sound.clip;
            BGM_Source.loop = true;

            BGM_Source.Play();
        }
    }

    public void StopBGM()
    {
        BGM_Source.Stop();
    }

    public void StopAllSound()
    {
        SFX_Source.Stop();
        BGM_Source.Stop();
    }
}
