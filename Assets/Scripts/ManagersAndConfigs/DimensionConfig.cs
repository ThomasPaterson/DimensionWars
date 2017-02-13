using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionConfig : MonoBehaviour
{

    public static DimensionConfig instance;

    public float gravity;
    public Dimension[] dimensions;

    private void Awake()
    {
        instance = this;
    }



}
