using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundConfig : MonoBehaviour
{

    public static SoundConfig instance;

    public enum Type { UiClick = 0, UiSelect = 1, UiCancel = 2}

    public SoundInfo[] soundInfo;
    public StudioEventEmitter music;
    public StudioEventEmitter intro;
    public StudioEventEmitter outro;
    public float introSuppress;
    public float outroSuppress;
    public float volumeRecoveryRate;

    private float musicTarget = 1f;
    private Dictionary<Type, StudioEventEmitter> emitterDict = new Dictionary<Type, StudioEventEmitter>();
    private bool suppressing;
    private float suppressionTime;


    private void Awake()
    {
        if (instance == null)
        {
            music.Play();
            SetMusicTarget(1f);
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < soundInfo.Length; i++)
                emitterDict.Add(soundInfo[i].type, soundInfo[i].eventEmitter);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!suppressing && musicTarget < 1f)
            SetMusicTarget(musicTarget + Time.deltaTime * volumeRecoveryRate);
       
    }


    public void PlayUiSound(Type type)
    {
        emitterDict[type].Play();
    }

    public void PlayIntroSound()
    {
        intro.Play();
        SuppressMusic(introSuppress);
    }

    public void PlayOutroSound()
    {
        outro.Play();
        SuppressMusic(outroSuppress);
    }

    void SuppressMusic(float length)
    {
        suppressionTime = length;

        if (!suppressing)
            StartCoroutine(HandleSuppression());
    }

    IEnumerator HandleSuppression()
    {
        suppressing = true;
        SetMusicTarget(0.5f);

        while (suppressionTime >= 0f)
        {
            
            suppressionTime -= Time.deltaTime;
            yield return null;
        }
        
        suppressing = false;
    }

    void SetMusicTarget(float newValue)
    {
        musicTarget = Mathf.Min(newValue, 1f);
        music.SetParameter("Volume", musicTarget);
    }
}

[System.Serializable]
public class SoundInfo
{
    public SoundConfig.Type type;
    public StudioEventEmitter eventEmitter;
}