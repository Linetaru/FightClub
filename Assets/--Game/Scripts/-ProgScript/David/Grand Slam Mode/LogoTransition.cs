using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class LogoTransition : MonoBehaviour
{
    [Title("Infos")]
    [SerializeField]
    private Image imageTransition;
    [SerializeField]
    private TextMeshProUGUI ruleText;

    [Title("Sprites")]
    [SerializeField]
    private Sprite logoClassicMode;
    [SerializeField]
    private Sprite logoBombMode;
    [SerializeField]
    private Sprite logoWallSplashMode;
    [SerializeField]
    private Sprite logoVolleyMode;

    [Title("Quick Rules")]
    [SerializeField]
    private string classicRules;
    [SerializeField]
    private string bombRules;
    [SerializeField]
    private string wallSplashRules;
    [SerializeField]
    private string volleyRules;

    private int scoreGoal;

    public void PlayTransition(GameModeStateEnum gameMode)
    {
        ChooseGameModeImage(gameMode);
        GetComponent<Animator>().Play("LogoTransition");
    }

    private void ChooseGameModeImage(GameModeStateEnum gameMode)
    {
        if (gameMode == GameModeStateEnum.Classic_Mode)
        {
            imageTransition.sprite = logoClassicMode;
            ruleText.text = classicRules;
        }
        else if (gameMode == GameModeStateEnum.Bomb_Mode)
        {
            imageTransition.sprite = logoBombMode;
            ruleText.text = bombRules;
        }
        else if (gameMode == GameModeStateEnum.Flappy_Mode)
        {
            imageTransition.sprite = logoWallSplashMode;
            ruleText.text = wallSplashRules;
        }
        else if (gameMode == GameModeStateEnum.Volley_Mode)
        {
            imageTransition.sprite = logoVolleyMode;
            ruleText.text = "Score " + scoreGoal + " points";
        }
    }

    public void SetScoreGoal(int goal)
    {
        scoreGoal = goal;
    }

    public void EndTransition()
    {
        gameObject.SetActive(false);
    }
}
