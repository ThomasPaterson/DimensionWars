using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{

	public void LoadGame()
    {
        SoundConfig.instance.PlayUiSound(SoundConfig.Type.UiClick);
        SceneManager.LoadScene(1);
    }

    public void LoadMenu()
    {
        SoundConfig.instance.PlayUiSound(SoundConfig.Type.UiClick);
        SceneManager.LoadScene(0);
    }
}
