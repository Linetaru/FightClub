using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrandSlamUi : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreInfosPanel;

    public List<GameObject> playersScoreObj = new List<GameObject>();
    public List<TextMeshProUGUI> playerNameTxt = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> playerScoreTxt = new List<TextMeshProUGUI>();


    public void ActivePanelScore()
    {
        scoreInfosPanel.SetActive(true);
    }

    public void DeactivePanelScore()
    {
        for (int i = 0; i < playersScoreObj.Count; i++)
        {
            playersScoreObj[i].SetActive(false);
        }
        scoreInfosPanel.SetActive(false);
    }

    public void DrawScores(int[] playersScore, int realLength)
    {
        for(int i = 0; i < realLength; i++)
        {
            playersScoreObj[i].SetActive(true);
            playerScoreTxt[i].text = playersScore[i] + "";
        }
    }
}
