using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GetShadowsFromSingleton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<ShadowCaster2D>().castsShadows = MainManager.Instance.areShadowsOn;
    }
}
