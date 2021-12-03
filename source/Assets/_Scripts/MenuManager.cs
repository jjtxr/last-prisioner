using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public Text playerText;
    public Text CO_buttonText;
    public GameObject[] characters;
    public GameObject descriptionHolder;
    public Text description;
    public string[] charDescription;
    private int currentPlayer = 1;

    private int gameModeIndex;

    public void CharacterSelect(int index)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (i == index)
            {
                characters[i].SetActive(true);
                descriptionHolder.SetActive(true);
                description.text = charDescription[i];
                SaveCharacter(i);
            }
            else
            {
                characters[i].SetActive(false);
            }
        }
    }

    public void SaveCharacter(int value)
    {
        if (gameModeIndex == 1)
            PlayerPrefs.SetInt("CS", value);
        else
        {
            PlayerPrefs.SetInt("CSP" + currentPlayer, value);
            Debug.Log("CSP" + currentPlayer + " = " + value);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadGameMode()
    {
        SceneManager.LoadScene(gameModeIndex);
    }

    public void SetGameMode(int index)
    {
        gameModeIndex = index;
    }

    public void ChooseCharacter()
    {
        currentPlayer++;
        PlayerPrefs.SetInt("CSP" + currentPlayer, PlayerPrefs.GetInt("CSP" + (currentPlayer-1).ToString()));
        playerText.text = "Player2 Turn";
        CO_buttonText.text = "Start";
        if (currentPlayer > 2)
        {
            LoadGameMode();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
