using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PauseGame : MonoBehaviour
{
    private bool isPause = false;

    public GameObject parentPauseUi;
    public TextMeshProUGUI textReturn;
    public TextMeshProUGUI textQuit;

    public InputController inputController;

    private int state = 0;

    public string quit_button_scene;

    private bool OnTransition;
    private float timeTransit;
    float tempScale;

    private enum State {
        Up,
        Down
    }

	public void UpdatePauseState()
    {
        isPause = !isPause;
        PauseGameUI();
    }

    private void Update()
    {
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

        if (isPause && !OnTransition)
        {
            for (int i = 0; i < inputController.playerInputs.Length; i++)
            {
                if (inputController.playerInputs[i].vertical > 0.75 || inputController.playerInputs[i].horizontal < -0.75)
                {
                    GetPositionCursor(State.Down);
                }
                else if (inputController.playerInputs[i].horizontal > 0.75 || inputController.playerInputs[i].vertical < -0.75)
                {
                    GetPositionCursor(State.Up);
                }
            }
        }

        if (isPause)
        {
            //Debug.Log("Encore un debug qui va finir par faire n'importe quoi ou pas c'est pas trop long j'espere comme debug");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (state == 0)
                    UpdatePauseState();
                else
                {
                    Time.timeScale = 1;
                    UnityEngine.SceneManagement.SceneManager.LoadScene(quit_button_scene);
                }
            }
        }
    }

    private void GetPositionCursor(State e_state)
    {
        //switch(e_state)
        //{
        //    case State.Up:
        //        if (state == 0)
        //            state = 1;
        //        else
        //            state = 0;
        //        break;

        //    case State.Down:

        //        break;
        //}

        OnTransition = true;

        if (state == 0)
        {
            state = 1;
            textQuit.transform.DOScale(new Vector3(2, 2, 2), 0.01f).SetUpdate(true);
            textReturn.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
        }
        else
        {
            state = 0;
            textReturn.transform.DOScale(new Vector3(2, 2, 2), 0.01f).SetUpdate(true);
            textQuit.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
        }
    }

    private void PauseGameUI()
    {
        if(Time.timeScale != 0)
            tempScale = Time.timeScale;

        parentPauseUi.SetActive(isPause);
        textReturn.transform.DOScale(new Vector3(2, 2, 2), 0.01f).SetUpdate(true);
        textQuit.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);

        if(isPause)
            Time.timeScale = 0;
        else
            Time.timeScale = tempScale;
    }
}