using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public string[] charactersName;
    public GameObject[] characters;
    public Sprite[] charactersA;
    public int roundsToWin;
    public float startDelay;
    public float endDelay;
    public Text messageText;
    public PlayerManager[] players;
    public GameObject horde;
    public GameObject[] boss;
    public Transform[] spawnPointMobs;
    public Transform[] spawnPointBoss;

    private int roundNumber;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private PlayerManager roundWinner;
    private PlayerManager gameWinner;

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnPlayers();


        StartCoroutine(GameLoop());
    }

    private void SpawnPlayers()
    {
        
        for (int i = 0; i < players.Length; i++)
        {
            players[i].playerNumber = i + 1;
            int c = LoadPlayerCharacter(players[i].playerNumber);
            players[i].CharInt = c;
            players[i].playersCharacterName.text = charactersName[c];
            players[i].playerCharacter = characters[c];
            players[i].ablityImage.sprite = charactersA[c];
            players[i].instance = Instantiate(players[i].playerCharacter, players[i].spawnPoint.position, players[i].spawnPoint.rotation) as GameObject;
            players[i].Setup();
        }
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (gameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundStarting()
    {
        ResetPlayers();
        DisablePlayerControl();
        SpawnMobs();
        roundNumber++;
        messageText.text = "Round " + roundNumber;
        yield return startWait;
    }

    private IEnumerator RoundPlaying()
    {
        EnablePlayerControl();
        messageText.text = string.Empty;

        while (!OnePlayerLeft())
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        DisablePlayerControl();
        roundWinner = null;
        roundWinner = GetRoundWinner();
        if (roundWinner != null)
            roundWinner.nWins++;
        gameWinner = GetGameWinner();
        string message = EndMessage();
        messageText.text = message;
        yield return endWait;
    }

    private bool OnePlayerLeft()
    {
        int numPlayersLeft = 0;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].instance.activeSelf)
                numPlayersLeft++;
        }
        return numPlayersLeft <= 1;
    }

    private PlayerManager GetRoundWinner()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].instance.activeSelf)
                return players[i];
        }

        return null;
    }

    private PlayerManager GetGameWinner()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].nWins == roundsToWin)
                return players[i];
        }

        return null;
    }

    private string EndMessage()
    {
        string message = "DRAW!";

        if (roundWinner != null)
            message = roundWinner.playerText + " WON THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < players.Length; i++)
        {
            message += players[i].playerText + ": " + players[i].nWins + " WON\n";
        }

        if (gameWinner != null)
            message = gameWinner.playerText + " WON THE GAME!";

        return message;
    }

    private void ResetPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Reset();
        }
    }

    private void EnablePlayerControl()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].EnableControl();
        }
    }

    private void DisablePlayerControl()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].DisableControl();
        }
    }

    private void SpawnMobs()
    {
        for (int i = 0; i < spawnPointMobs.Length; i++)
        {
            Instantiate(horde, spawnPointMobs[i].position, spawnPointMobs[i].rotation);
        }
        for (int i = 0; i < spawnPointBoss.Length; i++)
        {
            Instantiate(boss[i], spawnPointBoss[i].position, spawnPointBoss[i].rotation);
        }
    }

    private int LoadPlayerCharacter(int player)
    {
        int cs = 0;
        if (PlayerPrefs.HasKey("CSP" + player))
        {
            cs = PlayerPrefs.GetInt("CSP" + player);
            PlayerPrefs.DeleteKey("CSP" + player);
        }

        return cs;
    }

}
