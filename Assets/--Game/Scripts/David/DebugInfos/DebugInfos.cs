using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DebugInfos : MonoBehaviour
{
    [SerializeField]
    private List<CharacterBase> playersList = new List<CharacterBase>();

    [SerializeField]
    private PlayerInfos[] playerInfos;

    [SerializeField]
    private InputController inputController;

    bool startCount = false;
    int nextPos = 1;
    float frameAttackActive = 0;

    /*void Start()
    {
    }*/


    void Update()
    {
        if(startCount)
            frameAttackActive += Time.deltaTime;

        ShowHideInfos();

        SwitchPlayers();

        UpdateInfos();
    }

    public void AddCharacter(CharacterBase character)
    {
        playersList.Add(character);
        character.OnStateChanged += OnStateChangedCallback;
        character.Action.OnAttack += OnAttackCallback;
        character.Action.OnAttackActive += OnAttackActiveCallback;

        InitInfos();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < playersList.Count; i++)
        {
            playersList[i].OnStateChanged -= OnStateChangedCallback;
            playersList[i].Action.OnAttack -= OnAttackCallback;
            playersList[i].Action.OnAttackActive -= OnAttackActiveCallback;
        }
    }

    private void OnStateChangedCallback(CharacterState oldState, CharacterState newState)
    {
        if(newState is CharacterStateActing)
        {
            startCount = true;
        }
        else 
        {
            startCount = false;
        }

    }

    private void OnAttackCallback(AttackManager attackManager)
    {
        frameAttackActive = 0;
        startCount = true;
    }

    private void OnAttackActiveCallback()
    {
        startCount = false;
    }



    private void InitInfos()
    {
        for(int i = 0; i < playersList.Count; i++)
        {
            playerInfos[i].PlayerName.text = playersList[i].gameObject.name;
        }
    }

    private void UpdateInfos()
    {
        for (int i = 0; i < playersList.Count; i++)
        {

            playerInfos[i].CurrentState.text = playersList[i].CurrentState.name;
            playerInfos[i].SpeedX.text = playersList[i].Movement.SpeedX.ToString();
            playerInfos[i].SpeedY.text = playersList[i].Movement.SpeedY.ToString();
            playerInfos[i].StartupText.text = ((int)(frameAttackActive * 60)).ToString();

            // A Update avec la liste entière
            if (playersList[i].Input != null && playersList[i].Input.inputActions.Count > 0)
            {
                playerInfos[i].Inputs.text = playersList[i].Input.inputActions[0].action.name;
            }

        }
    }

    private void ShowHideInfos()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            for(int i = 0; i < playersList.Count; i++)
            {
                CanvasGroup canvasG = playerInfos[i].GetComponent<CanvasGroup>();
                if (canvasG.alpha < 1f)
                    canvasG.alpha = 1f;
                else
                    canvasG.alpha = 0f;
            }
        }
    }

    private void SwitchPlayers()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Switch Players");

            IControllable tmp = inputController.controllable[0];

            inputController.controllable[0] = inputController.controllable[nextPos];
            inputController.controllable[nextPos] = tmp;

            if (nextPos < playersList.Count - 1)
                nextPos++;
            else
                nextPos = 1;

        }
    }



}
