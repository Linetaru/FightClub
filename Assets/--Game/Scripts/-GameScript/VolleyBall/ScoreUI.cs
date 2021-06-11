using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI redTeamText;

    [SerializeField]
    TextMeshProUGUI blueTeamText;

    void UpdateScoreText(TextMeshProUGUI text, int value)
    {
        text.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .1f)
            .OnComplete(()=> 
            {
                text.transform.DOScale(new Vector3(1f, 1f, 1f), .1f);
            });
        text.text = value.ToString();
    }

    public void UpdateRedScoreText(int value)
    {
        UpdateScoreText(redTeamText, value);
    }

    public void UpdateBlueScoreText(int value)
    {
        UpdateScoreText(blueTeamText, value);
    }
}
