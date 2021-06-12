using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlamLogoMode : MonoBehaviour
{
    [SerializeField]
    private Sprite logoClassicMode;
    [SerializeField]
    private Sprite logoBombMode;
    [SerializeField]
    private Sprite logoWallSplashMode;
    [SerializeField]
    private Sprite logoVolleyMode;


    [SerializeField]
    private Image logoMode;

    private float randomSpeed = 0.1f;

    private bool wheelTurning;

    public void TriggerWheel()
    {
        StartCoroutine(TurnWheel());
    }

    public void DrawLogo(GameModeStateEnum gameMode)
    {
        wheelTurning = false;

        if(gameMode == GameModeStateEnum.Classic_Mode)
        {
            logoMode.sprite = logoClassicMode;
        }
        else if(gameMode == GameModeStateEnum.Bomb_Mode)
        {
            logoMode.sprite = logoBombMode;
        }
        else if (gameMode == GameModeStateEnum.Flappy_Mode)
        {
            logoMode.sprite = logoWallSplashMode;
        }
        else if (gameMode == GameModeStateEnum.Volley_Mode)
        {
            logoMode.sprite = logoVolleyMode;
        }
    }

    private IEnumerator TurnWheel()
    {
        int i = 0;
        wheelTurning = true;
        List<Sprite> spriteList = new List<Sprite> { logoClassicMode, logoBombMode, logoWallSplashMode, logoVolleyMode };

        while (wheelTurning)
        {
            logoMode.sprite = spriteList[i];

            if(i >= spriteList.Count - 1)
            {
                i = 0;
            }
            else
            {
                i++;
            }

            yield return new WaitForSeconds(randomSpeed);
        }
    }
}
