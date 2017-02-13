using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float maxShake;
    public float shakeAmount;
    public float shakeModifier = 1f;

    private float shakeLeft;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _transform.localPosition = Vector3.zero;
    }

    public void AddShake()
    {
        shakeLeft = shakeAmount;
    }

    private void Update()
    {
        shakeLeft -= Time.deltaTime;

        if (shakeLeft > 0f)
        {
            float shakePower = shakeLeft / shakeAmount;
            _transform.localPosition = new Vector3(UnityEngine.Random.Range(-maxShake, maxShake) * shakePower * shakeModifier, UnityEngine.Random.Range(-maxShake, maxShake) * shakePower * shakeModifier);
        }
        else
        {
            _transform.localPosition = Vector3.zero;
        }
    }
}
