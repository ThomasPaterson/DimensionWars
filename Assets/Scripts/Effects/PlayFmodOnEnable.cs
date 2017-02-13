using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class PlayFmodOnEnable : MonoBehaviour
{


    void OnEnable()
    {
        GetComponent<StudioEventEmitter>().Play();
    }
}
