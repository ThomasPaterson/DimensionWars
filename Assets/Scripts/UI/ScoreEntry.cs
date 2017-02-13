using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEntry : MonoBehaviour
{

    public int playerNum;

 

    public void DisplayPlayerWins()
    {
        Player player = PlayerManager.instance.players[playerNum];

        if (player.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);

            Image[] images = GetComponentsInChildren<Image>(true);

            for (int i = 0; i < images.Length; i++)
            {
                if (player.wins > i)
                {
                    images[i].color = DimensionConfig.instance.dimensions[playerNum].color;
                    images[i].enabled = true;
                }
                else
                {
                    images[i].enabled = false;
                }
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
