using UnityEngine;
using System.Collections;


public class TweenUiOnEnable : MonoBehaviour
{
    public Vector2 tweenOffset;
    public AnimationCurve moveCurve;
    public float timeToTake;
    public bool initialized { get; private set; }

    private Vector2 startPos;
    

    void OnEnable()
    {
        if (!initialized)
        {
            initialized = true;
            startPos = GetComponent<RectTransform>().anchoredPosition;
        }
            

        StartCoroutine(TweenOnEnable());
    }

    IEnumerator TweenOnEnable()
    {
        float timePassed = 0f;
        SetPosition(timePassed / timeToTake);
        yield return null;
        yield return null;

        while (timePassed < timeToTake)
        {
            timePassed += Time.unscaledDeltaTime;
            SetPosition(timePassed / timeToTake);
            yield return null;
        }

        SetPosition(1f);
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
}
