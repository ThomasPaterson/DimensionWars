using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;

    public GameObject characterPrefab;
    public Player[] players;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < players.Length; i++)
            if (players[i].gameObject.activeInHierarchy)
                players[i].SpawnPlayer();
    }

    public void ResetPlayers()
    {
        for (int i = 0; i < players.Length; i++)
            players[i].wins = 0;
    }
}
