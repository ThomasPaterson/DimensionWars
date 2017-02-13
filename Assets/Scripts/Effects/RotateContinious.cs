using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateContinious : MonoBehaviour
{

    public float rotationRate;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    void Update ()
    {
        _transform.Rotate(new Vector3(0f, 0f, rotationRate * Time.deltaTime));
	}
}
