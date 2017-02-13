using UnityEngine;
using System.Collections;


public class TweenUiContinousOnEnable : MonoBehaviour
{
    public Vector2 tweenOffset;
    public AnimationCurve moveCurve;
    public float timeToLoop;
    public bool initialized { get; private set; }

    public bool tweening;
    private Vector2 startPos;
    private float timePassed = 0f;
    

    void OnEnable()
    {
        if (!initialized)
        {
            initialized = true;
            startPos = GetComponent<RectTransform>().anchoredPosition;
        }

        timePassed = 0f;
    }

    private void Update()
    {
        timePassed += Time.unscaledDeltaTime;

        if (tweening || Vector2.Distance(startPos, GetComponent<RectTransform>().anchoredPosition) > 2f)
        {
            SetPosition(timePassed / timeToLoop);
        }
        else
            SetPosition(0f);
    }

    void SetPosition(float percentDone)
    {
        Vector2 newPos = startPos + tweenOffset * moveCurve.Evaluate(percentDone);
        GetComponent<RectTransform>().anchoredPosition = newPos;
    }

    public Vector2 GetStartPos()
    {
        return startPos;
    }

    public void SetTweening(bool state)
    {
        tweening = state;
    }
}
