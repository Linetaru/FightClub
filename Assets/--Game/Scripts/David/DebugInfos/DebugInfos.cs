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

    bool startCountKnockback = false;
    float knockbackDuration = 0;

    /*void Start()
    {
    }*/


    void Update()
    {
        if(startCount)
            frameAttackActive += Time.deltaTime;
        if (startCountKnockback)
        {
            float best = playersList[0].Knockback.KnockbackDuration;
            for (int i = 0; i < playersList.Count; i++)
            {
                knockbackDuration = Mathf.Max(best, playersList[i].Knockback.KnockbackDuration);
            }
        }

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
        character.Knockback.OnKnockback += OnKnockbackCount;

        InitInfos();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < playersList.Count; i++)
        {
            playersList[i].OnStateChanged -= OnStateChangedCallback;
            playersList[i].Action.OnAttack -= OnAttackCallback;
            playersList[i].Action.OnAttackActive -= OnAttackActiveCallback;
            playersList[i].Knockback.OnKnockback -= OnKnockbackCount;
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

        if (oldState is CharacterStateKnockback && !(newState is CharacterStateKnockback))
        {
            startCountKnockback = false;
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

    private void OnKnockbackCount(AttackSubManager attackManager)
    {
        startCountKnockback = true;
        float best = playersList[0].Knockback.KnockbackDuration;
        for (int i = 0; i < playersList.Count; i++)
        {
            knockbackDuration = Mathf.Max(best, playersList[i].Knockback.KnockbackDuration);
        }
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
            playerInfos[i].knockbackTime.localScale = new Vector3(knockbackDuration / playersList[i].Knockback.MaxTimeKnockback, 1, 1);

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
