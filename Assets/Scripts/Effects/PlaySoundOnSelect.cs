using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PlaySoundOnSelect : MonoBehaviour, ISelectHandler
{
    public SoundConfig.Type type;

    public void OnSelect(BaseEventData eventData)
    {
        if (GetComponent<Button>() == null || GetComponent<Button>().interactable)
            PlaySound();
    }

    public void PlaySound()
    {
        SoundConfig.instance.PlayUiSound(type);
    }
}
