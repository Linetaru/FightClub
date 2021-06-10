using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyMode : GameMode
{
    BattleManager battleManager;

    [SerializeField]
    ScoreUI scoreUI;
    [SerializeField]
    CharacterBase ball;

    int blueTeamScore = 0;
    int redTeamScore = 0;

    /*public PackageCreator.Event.GameEvent eventScoreRed;
    public PackageCreator.Event.GameEvent eventScoreBlue;*/

    [SerializeField]
    int scoreLimit = 5;

    // Start is called before the first frame update
    void Start()
    {
        battleManager = BattleManager.Instance;
        scoreUI.UpdateBlueScoreText(blueTeamScore);
        scoreUI.UpdateRedScoreText(redTeamScore);
        battleManager.cameraController.targets.Add(new TargetsCamera(ball.transform, 0));

        
        // A changer si on veut set l'objectif de points via le menu (on garde juste la première condition)
        if (battleManager.gameData.slamMode)
            scoreLimit = battleManager.gameData.GetModeScoreGoal(GameModeStateEnum.Volley_Mode);
        else
            battleManager.gameData.SetModeScoreGoal(GameModeStateEnum.Volley_Mode, scoreLimit);
    }

    public void UpdateRedScore()
    {
        redTeamScore++;
        scoreUI.UpdateRedScoreText(redTeamScore);
        if(redTeamScore >= scoreLimit)
        {
            battleManager.currentWinningTeam = 1;
            battleManager.SlowMotionEnd();
        }
    }

    public void UpdateBlueScore()
    {
        blueTeamScore++;
        scoreUI.UpdateBlueScoreText(blueTeamScore);
        if(blueTeamScore >= scoreLimit)
        {
            battleManager.currentWinningTeam = 0;
            battleManager.SlowMotionEnd();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
