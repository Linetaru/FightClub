﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System.Linq;

public class GrandSlamUi : MonoBehaviour
{
    [Title("Objects")]
    [SerializeField]
    private GameObject scoreInfosPanel;
    [SerializeField]
    private GameObject logoTransitionPanel;

    [Title("Components")]
    [SerializeField]
    private Animator backgroundAnimator;

    [Title("UI Components")]
    [SerializeField]
    private TextMeshProUGUI scoreToBeat;
    [SerializeField]
    private TextMeshProUGUI currentModeText;
    [SerializeField]
    private Image currentModeImage;


    [Title("Scripts")]
    [SerializeField]
    private LogoTransition logoTransition;

    [Title("List")]
    public List<GameObject> playersScoreObj = new List<GameObject>();
    public List<TextMeshProUGUI> playerNameTxt = new List<TextMeshProUGUI>();
    public List<Image> playerImage = new List<Image>();
    public List<TextMeshProUGUI> playerScoreTxt = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> playerGainScoreTxt = new List<TextMeshProUGUI>();

    [Title("Sprites")]
    [SerializeField]
    private Sprite logoClassicMode;
    [SerializeField]
    private Sprite logoBombMode;
    [SerializeField]
    private Sprite logoWallSplashMode;
    [SerializeField]
    private Sprite logoVolleyMode;


    public List<Image> crownList = new List<Image>();
    public List<Image> glowList = new List<Image>();

    int[] oldPlayersScore = new int[4] { 0, 0, 0, 0 };

    private int incRate = 4;

    List<int> bestScoreIDs = new List<int>();
    List<int> bestScoreIndexes = new List<int>();

    Color color = new Color();
    List<int> sortedControllerID = new List<int>();


    //Pour David : Enum pour gerer le classement et les égalités
    enum ScoreState{
        FirstPlace = 0,
        SecondPlace = 1,
        ThirdPlace = 2,
        FourthPlace = 3,
    }

    ScoreState[] playerState = new ScoreState[4] { 0, 0, 0, 0 };

    public void InitProperty(int playerScoreToBeat, GameData gameData)
    {
        scoreToBeat.text = playerScoreToBeat.ToString();

        for (int i = 0; i < gameData.CharacterInfos.Count; i++)
        {
            if (gameData.CharacterInfos[i].InputMapping.profileName == "classic")
            {
                playerNameTxt[i].text = gameData.CharacterInfos[i].CharacterData.characterName + " (J" + (i + 1) + ")";
            }
            else
            {
                playerNameTxt[i].text = gameData.CharacterInfos[i].InputMapping.profileName + " (J" + (i + 1) + ")";
            }
            playerImage[i].sprite = gameData.CharacterInfos[i].CharacterData.characterFace;
        }
    }

    public void ActivePanelScore()
    {
        scoreInfosPanel.SetActive(true);
        backgroundAnimator.SetTrigger("FadeIn");
    }

    public void DeactivePanelScore()
    {
        for (int i = 0; i < playersScoreObj.Count; i++)
        {
            playersScoreObj[i].SetActive(false);
        }
        scoreInfosPanel.SetActive(false);
    }


    public void SetCurrentModeInfo(GameModeStateEnum gameMode)
    {
        if(gameMode == GameModeStateEnum.Classic_Mode)
        {
            currentModeText.text = "CLASSIC";
            currentModeImage.sprite = logoClassicMode;
        }
        else if(gameMode == GameModeStateEnum.Bomb_Mode)
        {
            currentModeText.text = "BOMB";
            currentModeImage.sprite = logoBombMode;
        }
        else if (gameMode == GameModeStateEnum.Flappy_Mode)
        {
            currentModeText.text = "WALL SPLASH";
            currentModeImage.sprite = logoWallSplashMode;
        }
        else if (gameMode == GameModeStateEnum.Volley_Mode)
        {
            currentModeText.text = "VOLLEY";
            currentModeImage.sprite = logoVolleyMode;
        }
    }

    public void DrawScores(Dictionary<int, int> playersScore, GameData gameData)
    {
        int i = 0;
        bestScoreIndexes.Clear();
        bestScoreIDs.Clear();
        bestScoreIndexes.Add(0);

        sortedControllerID.Clear();
        foreach (KeyValuePair<int, int> item in playersScore.OrderBy(key => key.Value))
        {
            sortedControllerID.Add(item.Key);
        }
        sortedControllerID.Reverse();

        foreach (KeyValuePair<int, int> score in playersScore)
        {
            if (i == 0)
                bestScoreIDs.Add(score.Key);

            crownList[i].enabled = false;
            glowList[i].enabled = false;

            playersScoreObj[i].SetActive(true);


            if(score.Value > oldPlayersScore[i])
            {
                if(sortedControllerID[0] == score.Key)
                {
                    color = Color.yellow;
                }
                else if(sortedControllerID[1] == score.Key)
                {
                    color = Color.gray;
                }
                else if(sortedControllerID[2] == score.Key)
                {
                    color = new Color(0.2f, 0.3f, 0.4f);
                }
                else if(sortedControllerID[3] == score.Key)
                {
                    color = Color.red;
                }
                StartCoroutine(IncreaseScore(oldPlayersScore[i], score.Value, playerScoreTxt[i], color, playerGainScoreTxt[i]));

                oldPlayersScore[i] = score.Value;
            }

            if(i > 0)
            {
                if (score.Value > playersScore[bestScoreIDs[0]])
                {
                    bestScoreIDs.Clear();
                    bestScoreIDs.Add(score.Key);

                    bestScoreIndexes.Clear();
                    bestScoreIndexes.Add(i);
                }
                else if (score.Value == playersScore[bestScoreIDs[0]])
                {
                    bestScoreIDs.Add(score.Key);
                    bestScoreIndexes.Add(i);
                }
            }

            i++;
        }
        
        foreach(int index in bestScoreIndexes)
        {
            crownList[index].enabled = true;
            glowList[index].enabled = true;
        }

        /*
        for (int i = 0; i < realLength; i++)
        {
            playersScoreObj[i].SetActive(true);

            if(playersScore[i] > oldPlayersScore[i])
            {
                //Fancy Increase
                StartCoroutine(IncreaseScore(oldPlayersScore[i], playersScore[i], playerScoreTxt[i]));
            }
            else
            {
                // No Increase
            }

            // Récup la position du meilleur score au cas où on voudrait faire quelque chose avec le meilleur score
            if(playersScore[i] > playersScore[posBestScore])
            {
                posBestScore = i;
            }
        }

        crownList[posBestScore].enabled = true;

        oldPlayersScore = playersScore;
        */
    }

    private IEnumerator IncreaseScore(int from, int to, TextMeshProUGUI textScore, Color color, TextMeshProUGUI textGainScore)
    {
        int incValue = from;

        textScore.GetComponent<ScoreAnim>().TriggerAnim();

        textScore.color = color;
        textGainScore.gameObject.SetActive(true);
        textGainScore.text = "+" + (to - from);
        textGainScore.GetComponent<ScoreAnim>().TriggerAnim();
        textGainScore.color = color;

        while (incValue < to)
        {
            incValue += incRate;
            textScore.text = incValue.ToString();

            yield return new WaitForSeconds(0.02f);
        }

        textGainScore.GetComponent<ScoreAnim>().StopAnim();
        textGainScore.gameObject.SetActive(false);

        textScore.GetComponent<ScoreAnim>().StopAnim();

        textScore.text = to.ToString();

        // Increasing is done
    }

    public void StartTransitionLogo(GameModeStateEnum gameMode)
    {
        logoTransitionPanel.SetActive(true);
        logoTransition.PlayTransition(gameMode);
    }


}
