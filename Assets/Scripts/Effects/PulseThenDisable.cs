using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseThenDisable : MonoBehaviour
{
    public AnimationCurve pulseCurve;
    public float timeToPulse;
    private bool pulsing;
    private float height;
    private float period;
    private float timePassed = 0f;
    private Vector3 startSize;
    private Transform _transform;
    private bool initialized;


    private void OnEnable()
    {
        if (initialized)
            _transform.localScale = startSize;
        else
        {
            initialized = true;
            _transform = transform;
            startSize = _transform.localScale;
        }
    }

    private void OnDisable()
    {
        pulsing = false;
    }


    public void Pulse()
    {
        this.timePassed = 0f;
        gameObject.SetActive(true);

        if (!pulsing)
            StartCoroutine(HandlePulse());
    }

    IEnumerator HandlePulse()
    {
        pulsing = true;

        while (timePassed < timeToPulse)
        {
            timePassed += Time.deltaTime;
            _transform.localScale = startSize * pulseCurve.Evaluate(timePassed / timeToPulse);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
