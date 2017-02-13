using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulser : MonoBehaviour
{
    public AnimationCurve pulseCurve;

    private bool pulsing;
    private float height;
    private float period;
    private float timePassed = 0f;
    private Vector3 startSize;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        startSize = _transform.localScale;
    }

    private void OnDisable()
    {
        _transform.localScale = startSize;
    }

    public void Pulse(float height, float period)
    {
        this.height = height;
        this.period = period;
        this.timePassed = 0f;

        if (!pulsing)
            StartCoroutine(HandlePulse());
    }

	IEnumerator HandlePulse()
    {
        while (timePassed < period)
        {
            timePassed += Time.deltaTime;
            _transform.localScale = startSize * pulseCurve.Evaluate(timePassed / period) * height;
            yield return null;
        }

        _transform.localScale = startSize;
    }
}
