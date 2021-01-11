using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource SFX_Source, BGM_Source;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    SoundFile GetSound(AudioData _audioType, string _name)
    {
        List<SoundFile> temp = new List<SoundFile>(_audioType.audioList);
        
        for (int i = 0; i < temp.Count; i++)
        {
            if(temp[i].name == _name)
            {
                return temp[i];
            }
        }

        return null;
    }
    
    SoundFile GetRandomSound(AudioData _audioType, string _name)
    {
        List<SoundPack> temp = new List<SoundPack>(_audioType.soundPacks);

        for (int i = 0; i < temp.Count; i++)
        {
            if(temp[i].name == _name)
            {
                print("play");
                return temp[i].audioList[Random.Range(0, temp[i].audioList.Count)];
            }
        }
        
        return null;
    }

    public void PlaySFX(AudioData _audioData, string _name)
    {
        SoundFile sound = GetSound(_audioData, _name);
        if(sound != null)
        {
            SFX_Source.volume = sound.volume;
            SFX_Source.PlayOneShot(sound.clip);
        }
    }
    
    public void PlayRandomSFX(AudioData _audioData, string _name)
    {
        SoundFile sound = GetRandomSound(_audioData, _name);
        if(sound != null)
        {
            SFX_Source.volume = sound.volume;
            SFX_Source.PlayOneShot(sound.clip);
        }
    }
    
    public void PlayLoopingSFX(AudioData _audioData, string _name)
    {
        SoundFile sound = GetSound(_audioData, _name);
        if (sound != null)
        {
            SFX_Source.volume = sound.volume;
            SFX_Source.clip = sound.clip;
            SFX_Source.loop = true;

            SFX_Source.Play();
        }
    }
    
    public void PlayBGM(AudioData _audioData, string _name)
    {
        SoundFile sound = GetSound(_audioData, _name);
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
