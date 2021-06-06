using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PauseGame : MonoBehaviour, IControllable
{
    private bool isPause = false;

    public GameObject parentPauseUi;
    public TextMeshProUGUI textReturn;
    public TextMeshProUGUI textQuit;

    //public InputController inputController;

    private int state = 0;
    private int characterID = 0;

    public string quit_button_scene;

    private bool OnTransition;
    private float timeTransit;
    float tempScale;

    private enum State 
    {
        Up,
        Down
    }

    // Appelé par des events
	public void UpdatePauseState(int id)
    {
        if (BattleManager.Instance.GamePaused)
            return;
        characterID = id;
        isPause = true;
        BattleManager.Instance.SetMenuControllable(this);
        PauseGameUI();
    }

    public void UpdateControl(int id, Input_Info inputs)
    {
        if (id != characterID)
            return;

        if(OnTransition)
        {
            if (timeTransit < 0.5f)
                timeTransit += Time.unscaledDeltaTime;
            else
            {
                timeTransit = 0;
                OnTransition = false;
            }

        }

        if (isPause)
        {
            if (inputs.inputUiAction == InputConst.Interact)
            {
                if (state == 0)
                    ResumeGame();
                else
                {
                    Time.timeScale = 1;
                    UnityEngine.SceneManagement.SceneManager.LoadScene(quit_button_scene);
                }
            }
        }

        if(!OnTransition)
        {
            if (inputs.vertical > 0.75 || inputs.horizontal < -0.75)
            {
                GetPositionCursor(State.Down);
            }
            else if (inputs.horizontal > 0.75 || inputs.vertical < -0.75)
            {
                GetPositionCursor(State.Up);
            }
        }
    }

    private void GetPositionCursor(State e_state)
    {
        OnTransition = true;

        if (state == 0)
        {
            state = 1;
            textQuit.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.01f).SetUpdate(true);
            textReturn.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
        }
        else
        {
            state = 0;
            textReturn.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.01f).SetUpdate(true);
            textQuit.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
        }
    }

    private void PauseGameUI()
    {
        if(Time.timeScale != 0)
            tempScale = Time.timeScale;

        parentPauseUi.SetActive(isPause);
        textReturn.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.01f).SetUpdate(true);
        textQuit.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);

        if(isPause)
            Time.timeScale = 0;
        else
            Time.timeScale = tempScale;
    }


    private void ResumeGame()
    {
        isPause = false;
        BattleManager.Instance.SetBattleControllable();
        PauseGameUI();
    }
} 