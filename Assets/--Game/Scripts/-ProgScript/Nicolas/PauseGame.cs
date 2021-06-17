using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Menu;
using Sirenix.OdinInspector;

public class PauseGame : MonoBehaviour, IControllable
{
    private bool isPause = false;

    public GameObject parentPauseUi;
    public TextMeshProUGUI textReturn;
    public TextMeshProUGUI textOptions;
    public TextMeshProUGUI textQuit;
    public ButtonNavigationOptionsMenu optionsMenu;
    public InputOptionsMenu inputMenu;
    public GameObject canvasOptions;

    //public InputController inputController;

    private int state = 0;
    private int characterID = 0;

    [Scene]
    public string quit_button_scene;

    private bool OnTransition;
    private float timeTransit;
    float tempScale;


    [Title("Sounds")]
    [SerializeField]
    AK.Wwise.Event eventPauseOn;
    [SerializeField]
    AK.Wwise.Event eventPauseOff;
    [SerializeField]
    AK.Wwise.Event eventPauseStart;
    [SerializeField]
    AK.Wwise.Event eventPauseSelect;
    [SerializeField]
    AK.Wwise.Event eventPauseReturn;
    [SerializeField]
    AK.Wwise.Event eventPauseQuit;

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
                inputs.inputUiAction = null;
                if (state == 0)
                    ResumeGame();
                else if(state == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        BattleManager.Instance.inputController.controllable[i] = optionsMenu;
                    }
                    parentPauseUi.SetActive(false);
                    canvasOptions.SetActive(true);
                    optionsMenu.inputController = BattleManager.Instance.inputController;
                    inputMenu.inputController = BattleManager.Instance.inputController;
                    optionsMenu.InitializeMenu();
                    optionsMenu.OnEnd += OnEndOptionMenuInPauseCanvas;
                }
                else
                {
                    Time.timeScale = 1;
                    AkSoundEngine.PostEvent(eventPauseQuit.Id, this.gameObject);
                    AkSoundEngine.PostEvent(eventPauseOff.Id, this.gameObject);
                    if(BattleManager.Instance.gameData.slamMode)
                    {
                        BattleManager.Instance.gameData.GameMode = GameModeStateEnum.Special_Mode;
                    }
                    UnityEngine.SceneManagement.SceneManager.LoadScene(quit_button_scene);
                }
            }
        }

        if(!OnTransition)
        {
            if (inputs.vertical > 0.75 || inputs.horizontal < -0.75)
            {
                AkSoundEngine.PostEvent(eventPauseSelect.Id, this.gameObject);
                GetPositionCursor(State.Up);
            }
            else if (inputs.horizontal > 0.75 || inputs.vertical < -0.75)
            {
                AkSoundEngine.PostEvent(eventPauseSelect.Id, this.gameObject);
                GetPositionCursor(State.Down);
            }
        }
    }

    public void OnEndOptionMenuInPauseCanvas()
    {
        for (int i = 0; i < 4; i++)
        {
            BattleManager.Instance.inputController.controllable[i] = this;
        }
        for (int z = 0; z < BattleManager.Instance.gameData.CharacterInfos.Count; z++)
        {
            foreach (InputMappingDataClassic imDC in InputMappingDataStatic.inputMappingDataClassics)
            {
                if (BattleManager.Instance.gameData.CharacterInfos[z].InputMapping.profileName == imDC.profileName && BattleManager.Instance.gameData.CharacterInfos[z].InputMapping.isUsed)
                {
                    BattleManager.Instance.gameData.CharacterInfos[z].InputMapping = imDC;
                    BattleManager.Instance.gameData.CharacterInfos[z].InputMapping.isUsed = true;
                }
            }
        }
        parentPauseUi.SetActive(true);
        canvasOptions.SetActive(false);
    }

    private void GetPositionCursor(State e_state)
    {
        OnTransition = true;

        if(e_state == State.Up)
        {
            if (state == 0)
            {
                state = 2;
                textQuit.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.01f).SetUpdate(true);
                textReturn.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
            }
            else if(state == 1)
            {
                state = 0;
                textReturn.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.01f).SetUpdate(true);
                textOptions.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
            }
            else
            {
                state = 1;
                textOptions.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.01f).SetUpdate(true);
                textQuit.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
            }
        }
        else
        {
            if (state == 0)
            {
                state = 1;
                textOptions.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.01f).SetUpdate(true);
                textReturn.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
            }
            else if (state == 1)
            {
                state = 2;
                textQuit.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.01f).SetUpdate(true);
                textOptions.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
            }
            else
            {
                state = 0;
                textReturn.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.01f).SetUpdate(true);
                textQuit.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
            }
        }
    }

    private void PauseGameUI()
    {
        if(Time.timeScale != 0)
            tempScale = Time.timeScale;

        parentPauseUi.SetActive(isPause);
        textReturn.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.01f).SetUpdate(true);
        textOptions.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);
        textQuit.transform.DOScale(new Vector3(1, 1, 1), 0.01f).SetUpdate(true);

        if(isPause)
        {
            AkSoundEngine.PostEvent(eventPauseStart.Id, this.gameObject);
            AkSoundEngine.PostEvent(eventPauseOn.Id, this.gameObject);
            Time.timeScale = 0;
        }
        else
        {
            AkSoundEngine.PostEvent(eventPauseOff.Id, this.gameObject);
            Time.timeScale = tempScale;
        }

    }


    private void ResumeGame()
    {
        AkSoundEngine.PostEvent(eventPauseReturn.Id, this.gameObject);
        isPause = false;
        BattleManager.Instance.SetBattleControllable();
        PauseGameUI();
    }
} 