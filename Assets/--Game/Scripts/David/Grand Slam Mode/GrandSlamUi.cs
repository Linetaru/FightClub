using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class GrandSlamUi : MonoBehaviour
{
    [Title("Objects")]
    [SerializeField]
    private GameObject scoreInfosPanel;
    [SerializeField]
    private GameObject logoTransitionPanel;

    [Title("Scripts")]
    [SerializeField]
    private LogoTransition logoTransition;

    [Title("List")]
    public List<GameObject> playersScoreObj = new List<GameObject>();
    public List<TextMeshProUGUI> playerNameTxt = new List<TextMeshProUGUI>();
    public List<Image> playerImage = new List<Image>();
    public List<TextMeshProUGUI> playerScoreTxt = new List<TextMeshProUGUI>();

    public List<Image> crownList = new List<Image>();

    int[] oldPlayersScore = new int[4] { 0, 0, 0, 0 };

    private int incRate = 4;
    int posBestScore = 0;

    private void Update()
    {
        
    }

    public void ActivePanelScore()
    {
        scoreInfosPanel.SetActive(true);
    }

    public void DeactivePanelScore()
    {
        crownList[posBestScore].enabled = false;

        for (int i = 0; i < playersScoreObj.Count; i++)
        {
            playersScoreObj[i].SetActive(false);
        }
        scoreInfosPanel.SetActive(false);
    }

    public void DrawScores(Dictionary<int, int> playersScore, GameData gameData)
    {
        int i = 0;

        foreach(KeyValuePair<int, int> score in playersScore)
        {
            playersScoreObj[i].SetActive(true);

            playerNameTxt[i].text = gameData.CharacterInfos[i].CharacterData.characterName + " (J" + (i+1) + ")";
            playerImage[i].sprite = gameData.CharacterInfos[i].CharacterData.characterFace;

            if(score.Value > oldPlayersScore[i])
            {
                StartCoroutine(IncreaseScore(oldPlayersScore[i], score.Value, playerScoreTxt[i]));

                oldPlayersScore[i] = score.Value;
            }

            if (playersScore[i] > playersScore[posBestScore])
            {
                posBestScore = i;
            }

            crownList[posBestScore].enabled = true;

            i++;
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

    private IEnumerator IncreaseScore(int from, int to, TextMeshProUGUI textScore)
    {
        int incValue = from;

        textScore.GetComponent<ScoreAnim>().TriggerAnim();


        while (incValue < to)
        {
            incValue += incRate;
            textScore.text = incValue.ToString();

            yield return new WaitForSeconds(0.02f);
        }

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
