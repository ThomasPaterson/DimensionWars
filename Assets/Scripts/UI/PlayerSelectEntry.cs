using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectEntry : MonoBehaviour
{
    public int playerNum;
    public Image unselectedAvatar;
    public Image selectedAvatar;
    public GameObject unselectedHint;
    public GameObject selectedHint;

    private Player player;

    private void OnEnable()
    {
        player = PlayerManager.instance.players[playerNum];
        Sprite sprite = CharacterManager.instance.characterSprites[player.playerNum].avatar;
        unselectedAvatar.sprite = sprite;
        selectedAvatar.sprite = sprite;
        //GetComponent<Image>().color = DimensionConfig.instance.dimensions[playerNum].color;

        UpdateAppearance();
    }

    private void Update()
    {
        CheckInput();

        UpdateAppearance();
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("Dimension0_" + playerNum.ToString()))
        {
            SoundConfig.instance.PlayUiSound(SoundConfig.Type.UiClick);
            player.gameObject.SetActive(true);
        }
        else if (Input.GetButtonDown("Dimension1_" + playerNum.ToString()))
        {
            SoundConfig.instance.PlayUiSound(SoundConfig.Type.UiCancel);
            player.gameObject.SetActive(false);
        }
    }

    void UpdateAppearance()
    {
        unselectedAvatar.enabled = !player.gameObject.activeInHierarchy;
        selectedAvatar.enabled = player.gameObject.activeInHierarchy;
        unselectedHint.gameObject.SetActive(player.gameObject.activeInHierarchy);
        selectedHint.gameObject.SetActive(!player.gameObject.activeInHierarchy);
    }
}
