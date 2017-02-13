using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    public static GameStateManager instance;

    public int round;
    public bool playing;
    public int winsNeeded;
    public float delayAfterValidWinState = 1.5f;
    public GameMode currentMode;
    public GameMode[] gameModes;
    public bool usingModifiers;
    public float modifierChance;
    public bool useDebugModifier;
    public int debugModifier;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //StartCoroutine(PlayGame());
        UiManager.instance.playerChoicePanel.SetActive(true);
    }

    IEnumerator PlayGame()
    {
        currentMode = SelectMode();

        UiManager.instance.ShowRoundStart(round, currentMode.roundName);

        SoundConfig.instance.PlayIntroSound();

        MapManager.instance.CreateLevel();

        yield return null;


        PlayerManager.instance.SpawnPlayers();


        yield return null;

        while (UiManager.instance.displaying)
            yield return null;

        playing = true;

        while (!CheckFinished())
            yield return null;

        yield return new WaitForSeconds(delayAfterValidWinState);

        playing = false;

        SoundConfig.instance.PlayOutroSound();

        int winner = DetermineWinner();

        bool roundOver = false;

        if (winner != -1)
        {
            PlayerManager.instance.players[winner].wins = PlayerManager.instance.players[winner].wins + 1;
            roundOver = PlayerManager.instance.players[winner].wins >= winsNeeded;
        }


        UiManager.instance.ShowRoundEnd(round, winner);

        while (UiManager.instance.displaying)
            yield return null;

        MapManager.instance.Cleanup();
        CharacterManager.instance.Cleanup();
        
        round++;


        if (roundOver)
            StartCoroutine(FinishMatch());
        else
            StartCoroutine(PlayGame());
    }

    public void StartMatch()
    {
        UiManager.instance.playerChoicePanel.SetActive(false);
        StartCoroutine(PlayGame());
    }

    IEnumerator FinishMatch()
    {
        round = 0;

        UiManager.instance.ShowMatchEnd(DetermineMatchWinner());

        while (UiManager.instance.displaying)
            yield return null;

        PlayerManager.instance.ResetPlayers();

        UiManager.instance.playerChoicePanel.SetActive(true);
    }


    bool CheckFinished()
    {
        return CharacterManager.instance.characters.Count <= 1;
    }

    int DetermineWinner()
    {
        if (CharacterManager.instance.characters.Count > 0)
            return CharacterManager.instance.characters[0].controllingPlayer.playerNum;
        else
            return -1;
    }

    int DetermineMatchWinner()
    {
        for (int i = 0; i < PlayerManager.instance.players.Length; i++)
        {
            if (PlayerManager.instance.players[i].wins >= winsNeeded)
                return i;
        }

        return -1;
    }

    GameMode SelectMode()
    {
        if (usingModifiers)
        {
#if UNITY_EDITOR
            if (useDebugModifier)
                return gameModes[debugModifier];
#endif
            if (UnityEngine.Random.value <= modifierChance)
                return gameModes[UnityEngine.Random.Range(0, gameModes.Length)];
        }
        
        return gameModes[0];
    }
}
