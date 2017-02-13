using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectButtonOnEnable : MonoBehaviour
{

    private void OnEnable()
    {
        StartCoroutine(SelectAfterFrame());
     
    }

    private void Update()
    {

       
    }

    IEnumerator SelectAfterFrame()
    {
        yield return null;

        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {

            EventSystem.current.SetSelectedGameObject(null);
        }


        EventSystem.current.SetSelectedGameObject(gameObject);
    }


}
