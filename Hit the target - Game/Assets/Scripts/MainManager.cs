using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    // Audio variables.

    public bool isMusicMuted;
    public float volumeValue;
    public bool areSFXMuted;
    public float soundsValue;

    // Graphcis variables.

    public bool isPPOn;
    public bool areShadowsOn;
    public bool areVFXOn;

    // Start is called before the first frame update
    void Awake()
    {
        volumeValue = 0.5f;
        soundsValue = 0.5f;
        isMusicMuted = false;
        areSFXMuted = false;
        isPPOn = true;
        areShadowsOn = true;
        areVFXOn = true;
        
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Audio scene data persistance functions.
    public void SetMusic(bool musicToggleValue)
    {
        isMusicMuted = musicToggleValue;
    }

    public void SetVolume(float sliderValue)
    {
        volumeValue = sliderValue;
    }   
    public void SFXToggle(bool sfxToggleValue)
    {
        areSFXMuted = sfxToggleValue;
    }   
    public void SFXvalue(float sliderValue)
    {
        soundsValue = sliderValue;
    } 

    // Graphics scene data persistance functions.

    public void PPEnabled(bool PPToggleValue)
    {
        isPPOn = PPToggleValue;
    }
    public void ShadowsEnabled(bool ShadowsToggleValue)
    {
        areShadowsOn = ShadowsToggleValue;
    }
    public void VFXEnabled(bool VFXToggleValue)
    {
        areVFXOn = VFXToggleValue;
    }
}
