﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DebugInfos : MonoBehaviour
{
    [SerializeField]
    private CharacterBase[] playersList;

    [SerializeField]
    private PlayerInfos[] playerInfos;

    void Start()
    {
        InitInfos();
    }


    void Update()
    {
        ShowHideInfos();

        UpdateInfos();
    }

    private void InitInfos()
    {
        for(int i = 0; i < playersList.Length; i++)
        {
            playerInfos[i].PlayerName.text = playersList[i].gameObject.name;
        }
    }

    private void UpdateInfos()
    {
        for (int i = 0; i < playersList.Length; i++)
        {

            playerInfos[i].CurrentState.text = playersList[i].CurrentState.name;
            playerInfos[i].SpeedX.text = playersList[i].Movement.SpeedX.ToString();
            playerInfos[i].SpeedY.text = playersList[i].Movement.SpeedY.ToString();

            // A Update avec la liste entière
            if(playersList[i].Input.inputActions.Count > 0)
                playerInfos[i].Inputs.text = playersList[i].Input.inputActions[0].action.name;

        }
    }

    private void ShowHideInfos()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            for(int i = 0; i < playersList.Length; i++)
            {
                CanvasGroup canvasG = playerInfos[i].GetComponent<CanvasGroup>();
                if (canvasG.alpha < 1f)
                    canvasG.alpha = 1f;
                else
                    canvasG.alpha = 0f;
            }
        }
    }

}
