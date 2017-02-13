using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public static UiManager instance;

    public GameObject playerChoicePanel;
    public GameObject roundObject;
    public Text roundText;
    public Text roundModifierText;
    public GameObject winnerObject;
    public Text winnerText;
    public GameObject endMatchObject;
    public GameObject mainMenuObject;
    public Text endMatchText;
    public float roundStartTime;
    public float roundEndTime;
    public bool displaying;

    private void Awake()
    {
        instance = this;
        roundObject.SetActive(false);
        winnerObject.SetActive(false);
        endMatchObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Select"))
            mainMenuObject.SetActive(true);
    }

    public void ShowRoundStart(int round, string modifier)
    {
        StartCoroutine(HandleRoundStart(round, modifier));
    }

    public void ShowRoundEnd(int round, int winner)
    {
        StartCoroutine(HandleRoundEnd(round, winner));
    }

    public void ShowMatchEnd(int winner)
    {
        StartCoroutine(HandleMatchEnd(winner));
    }

    IEnumerator HandleRoundStart(int round, string modifier)
    {
        displaying = true;

        winnerObject.SetActive(false);
        roundText.text = "Round " + (round + 1).ToString();
        roundObject.SetActive(true);
        roundModifierText.text = modifier == "" ? "" : "Modifier: " + modifier;

        yield return new WaitForSeconds(roundStartTime);

        roundObject.SetActive(false);

        displaying = false;
    }

    IEnumerator HandleRoundEnd(int round, int winner)
    {
        displaying = true;
        
        winnerText.text = winner != -1 ? "Player " + (winner + 1).ToString() + " Wins!" : "Draw!";
        winnerObject.SetActive(true);

        foreach (ScoreEntry entry in winnerObject.GetComponentsInChildren<ScoreEntry>(true))
            entry.DisplayPlayerWins();

        yield return new WaitForSeconds(roundEndTime);

        winnerText.text = "";
        displaying = false;
    }

    IEnumerator HandleMatchEnd(int winner)
    {
        displaying = true;

        endMatchText.text = winner != -1 ? "Player " + (winner + 1).ToString() + "  Is Victorious!" : "Draw!";
        winnerObject.SetActive(false);
        roundObject.SetActive(false);
        endMatchObject.SetActive(true);

        foreach (ScoreEntry entry in endMatchObject.GetComponentsInChildren<ScoreEntry>(true))
            entry.DisplayPlayerWins();

        yield return new WaitForSeconds(1f);

        while (!Input.GetButton("Submit"))
            yield return null;

        endMatchObject.SetActive(false);
        displaying = false;
        
       
    }
}
