using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVolInMenu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.GetComponent<AudioSource>().mute = MainManager.Instance.isMusicMuted;
        this.GetComponent<AudioSource>().volume = MainManager.Instance.volumeValue;
    }
}
