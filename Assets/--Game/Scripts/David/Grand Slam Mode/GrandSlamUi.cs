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

    [Title("List")]
    public List<GameObject> playersScoreObj = new List<GameObject>();
    public List<TextMeshProUGUI> playerNameTxt = new List<TextMeshProUGUI>();
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

    public void DrawScores(int[] playersScore, int realLength)
    {
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

}
