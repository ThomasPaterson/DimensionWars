using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaySoundOnClick : MonoBehaviour, IPointerClickHandler
{
    public SoundConfig.Type type;

	public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<Button>() == null || GetComponent<Button>().interactable)
            PlaySound();
    }

    public void PlaySound()
    {
        SoundConfig.instance.PlayUiSound(type);
    }
}
