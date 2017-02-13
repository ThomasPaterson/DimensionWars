using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerConfig : MonoBehaviour
{
    public static LayerConfig instance;

    public LayerMask terrainLayers;
    // Use this for initialization

    private void Awake()
    {
        instance = this;
    }
}
