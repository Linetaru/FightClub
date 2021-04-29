using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : BattleManager
{
    [SerializeField]
    ScoreUI scoreUI;

    int blueTeamScore = 0;
    int redTeamScore = 0;

    public PackageCreator.Event.GameEvent eventScoreRed;
    public PackageCreator.Event.GameEvent eventScoreBlue;

    [SerializeField]
    int scoreLimit = 5;

    // Start is called before the first frame update
    void Start()
    {
        scoreUI.UpdateBlueScoreText(blueTeamScore);
        scoreUI.UpdateRedScoreText(redTeamScore);
        SpawnPlayer();
    }

    public void UpdateRedScore()
    {
        redTeamScore++;
        scoreUI.UpdateRedScoreText(redTeamScore);
        if(redTeamScore >= scoreLimit)
            StartCoroutine(EndBattleCoroutine());
    }

    public void UpdateBlueScore()
    {
        blueTeamScore++;
        scoreUI.UpdateBlueScoreText(blueTeamScore);
        if(blueTeamScore >= scoreLimit)
            StartCoroutine(EndBattleCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
