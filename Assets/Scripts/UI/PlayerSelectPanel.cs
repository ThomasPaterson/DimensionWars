using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectPanel : MonoBehaviour
{
    public GameObject startHint;
    public GameObject invalidHint;

    private bool validToStart;

    private void OnEnable()
    {
        if (GameStateManager.instance == null || PlayerManager.instance == null)
            gameObject.SetActive(false);

        validToStart = false;
    }

    private void Update()
    {
        validToStart = CheckValidToStart();

        startHint.SetActive(validToStart);
        invalidHint.SetActive(!validToStart);

        if (validToStart && Input.GetButtonDown("Start"))
            GameStateManager.instance.StartMatch();
    }

    bool CheckValidToStart()
    {
        int activePlayers = 0;

        for (int i = 0; i < PlayerManager.instance.players.Length; i++)
            if (PlayerManager.instance.players[i].gameObject.activeInHierarchy)
                activePlayers++;

        return activePlayers >= 2;
    }
    
	
}
