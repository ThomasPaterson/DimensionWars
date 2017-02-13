using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class PulseCanvasGroupOnEnable : MonoBehaviour
{

    public AnimationCurve pulseCurve;
    public bool disableAtZero;
    public bool canInteract = true;

    private CanvasGroup canvasGroup;

    void OnEnable()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
 

        StartCoroutine(Pulse());
    }
    IEnumerator Pulse()
    {
        float timePassed = 0f;

        SetValue(0f);
        yield return null;

        while (true)
        {
            timePassed += Time.unscaledDeltaTime;
            SetValue(timePassed);

            yield return null;
        }
    }

    void SetValue(float timePassed)
    {
        canvasGroup.alpha = pulseCurve.Evaluate(timePassed);

        if (canvasGroup.alpha <= 0f && disableAtZero)
            canvasGroup.interactable = false;
        else if (disableAtZero)
            canvasGroup.interactable = canInteract;
    }

}
