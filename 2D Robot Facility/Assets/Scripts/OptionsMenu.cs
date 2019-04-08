using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetVolume (float Volume)
    {
        audioMixer.SetFloat("Master", Volume);
    }

    public void SetBGMVolume(float BGMVolume)
    {
        audioMixer.SetFloat("BGM", BGMVolume);
    }

    public void SetSFXVolume(float SFXVolume)
    {
        audioMixer.SetFloat("SFX", SFXVolume);
    }

    public void SetQuality( int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
}
