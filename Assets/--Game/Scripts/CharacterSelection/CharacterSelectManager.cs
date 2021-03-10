using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField]
    PlayerSelectionFrame[] players;

    [SerializeField]
    GameObject readyBands;
    [SerializeField]
    GameObject readySlash;

    [SerializeField]
    GameData gameData;

    public int numberOfReadyPlayers = 0; //A changer et à faire avec des events
    public int numberOfConnectedPlayers = 0; //A changer et à faire avec des events

    public bool gameLaunched = false;

    int gameDuration = 8;
    int playerStocks = 3;

    public void DisplayReadyBands()
    {
        if (numberOfConnectedPlayers > 1 && numberOfReadyPlayers == numberOfConnectedPlayers)
            readyBands.SetActive(true);
    }

    public void HideReadyBands()
    {
        readyBands.SetActive(false);
    }

    public void PlayReadySlashAnimation()
    {
        if (!gameLaunched && numberOfReadyPlayers == numberOfConnectedPlayers && numberOfConnectedPlayers > 1)
        {
            gameLaunched = true;
            readySlash.SetActive(true);
            gameData.NumberOfLifes = playerStocks;

            StartCoroutine(GoToStageMenu());

            int characterInfoNumber = 0;

            gameData.CharacterInfos.Clear();

            for (int i = 0; i < numberOfReadyPlayers; i++)
            {
                gameData.CharacterInfos.Add(new Character_Info());
            }
            Debug.Log(players.Length);

            for (int i = 0; i < players.Length; i++)
            {
                Debug.Log(players[i].actualCursorPosition);

                if (players[i].isPlayerReady)
                {
                    Debug.Log(players[i].actualCursorPosition);
                    gameData.CharacterInfos[characterInfoNumber].CharacterData = players[i].characterCells[players[i].actualCursorPosition].characterData;
                    gameData.CharacterInfos[characterInfoNumber].CharacterColorID = players[i].actualColorSkin;
                    characterInfoNumber++;
                    //gameData.CharacterInfos[characterInfoNumber].CharacterData.
                }
            }
        }
    }

    IEnumerator GoToStageMenu()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("MenuSelection_Stage");
    }
}
