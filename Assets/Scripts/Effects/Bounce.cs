using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public AnimationCurve xCurve;
    public AnimationCurve yCurve;
    public float bounceTime;

    private Transform _transform;
    private Vector3 startScale;
    private bool initialized;
    private float timePassed = 0f;
    private bool bouncing;

    private void OnEnable()
    {
        if (initialized)
        {
            _transform.localScale = startScale;
            timePassed = 0f;
            bouncing = false;
        }
        else
        {
            initialized = true;
            _transform = transform;
            startScale = _transform.localScale;
        }
    }

    public void StartBounce()
    {
        timePassed = 0f;

        if (!bouncing)
            StartCoroutine(HandleBounce());
    }

    IEnumerator HandleBounce()
    {
        bouncing = true;

        while (timePassed < bounceTime)
        {
            timePassed += Time.deltaTime;
            _transform.localScale =  new Vector3(xCurve.Evaluate(timePassed / bounceTime), yCurve.Evaluate(timePassed / bounceTime));
            yield return null;
        }
        _transform.localScale = startScale;
        bouncing = false;
    }

}
