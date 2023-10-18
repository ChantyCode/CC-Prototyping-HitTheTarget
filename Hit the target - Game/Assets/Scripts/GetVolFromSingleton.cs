using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVolFromSingleton : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        this.GetComponent<AudioSource>().mute = MainManager.Instance.isMusicMuted;
        this.GetComponent<AudioSource>().volume = MainManager.Instance.volumeValue;
    }
}
