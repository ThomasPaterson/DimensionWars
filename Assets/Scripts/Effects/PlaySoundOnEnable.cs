using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaySoundOnEnable : MonoBehaviour
{
    public SoundConfig.Type type;

	void OnEnable()
    {
        if (SoundConfig.instance != null)
            SoundConfig.instance.PlayUiSound(type);
    }
}
