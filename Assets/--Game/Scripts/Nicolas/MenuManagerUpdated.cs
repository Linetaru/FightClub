using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.EventSystems;
using Rewired.UI.ControlMapper;
using AK;


public class MenuManagerUpdated : MonoBehaviour, IControllable
{
	public GameData gameData;

	[Title("Canvas Object Start")]
	public List<TextMeshProUGUI> startTexts;
	public List<GameObject> principalButtons;

	[Title("Canvas Object Mode")]
	public List<GameObject> modeButtons;

	[Title("Parameter Variable")]
	[ReadOnly] public int currentButtonSelected;
	bool OnTransition = false;
	float timeTransition = 0;

	[Title("Selected Button")]
	[ReadOnly] public GameObject currentSelectButton;
	[ReadOnly] public GameObject lastSelectButton;

	[Title("ControlMapper")]
	public ControlMapper controlMapper;

	[Title("Level Name")]
	public string sceneSelectionClassic;
	public string sceneSelectionGrandSlam;
	public string sceneSelectionVolley;
	public string sceneSelectionBomb;
	public string sceneSelectionFlappy;
	private string sceneName;

	[Title("Ak Wwise Sound Design")]
	public AK.Wwise.Event clickIn;
	public AK.Wwise.Event pressStart;
	public AK.Wwise.Event mainWhoosh;

	private enum MenuState
	{
		InStart,
		InPrincipalMenu,
		InModeMenu,
		InTransition,
	}

	private MenuState menuState = MenuState.InStart;

	private void Start()
	{
		controlMapper.Open();
		controlMapper.onScreenClosed -= CloseOptions;
		controlMapper.onScreenClosed += CloseOptions;
		controlMapper.Close(true);
	}

	public void UpdateControl(int ID, Input_Info input_Info)
	{

		if (timeTransition > 0)
		{
			timeTransition -= Time.deltaTime;
		}
		else
        {
			OnTransition = false;
		}

		//if (canChangeScene)
		//{
		//	GoToOtherScene();
		//}

		if (!controlMapper.isOpen && timeTransition <= 0)
		{
			if (!OnTransition && menuState != MenuState.InStart && menuState != MenuState.InTransition)
			{
				if (input_Info.vertical < -0.75)
					ChangeSelectedButton(false);
				else if (input_Info.vertical > 0.75)
					ChangeSelectedButton(true);

				if (input_Info.horizontal < -0.75)
					ChangeSelectedButton(true);
				else if (input_Info.horizontal > 0.75)
					ChangeSelectedButton(false);

				if (input_Info.CheckAction(0, InputConst.LeftTaunt) || input_Info.CheckAction(0, InputConst.UpTaunt))
				{
					input_Info.inputActions[0].timeValue = 0;
					ChangeSelectedButton(true);
				}
				else if (input_Info.CheckAction(0, InputConst.RightTaunt) || input_Info.CheckAction(0, InputConst.DownTaunt))
				{
					input_Info.inputActions[0].timeValue = 0;
					ChangeSelectedButton(false);
				}
			}

			if (input_Info.inputUiAction == InputConst.Pause && menuState == MenuState.InStart)
			{
				AkSoundEngine.PostEvent(pressStart.Id, this.gameObject);
				AkSoundEngine.PostEvent(mainWhoosh.Id, this.gameObject);

				menuState = MenuState.InPrincipalMenu;
				Camera.main.transform.gameObject.GetComponent<Animator>().enabled = true;
				foreach (TextMeshProUGUI text in startTexts)
					text.transform.gameObject.SetActive(false);
				//EventSystem.current.SetSelectedGameObject(playButton);
				currentButtonSelected = 1;
				currentSelectButton = principalButtons[0];
				currentSelectButton.SetActive(true);
			}

			if (menuState != MenuState.InStart && input_Info.inputUiAction == InputConst.Interact)
			{

				switch (menuState)
				{
					case MenuState.InPrincipalMenu:
						switch (currentButtonSelected)
						{
							case 1:
								Camera.main.GetComponent<Animator>().SetBool("canTransiToMode", true);
								menuState = MenuState.InModeMenu;
								currentButtonSelected = 2; 
								ChangeSelectedButton(true);
								//timeTransition = 999;
								//canChangeScene = true;
								break;
							case 2:
								Options();
								break;
							case 3:
								Application.Quit();
								break;
						}
						break;
					case MenuState.InModeMenu:
						switch (currentButtonSelected)
						{
							case 1:
								Camera.main.GetComponent<Animator>().SetBool("canTransiToMode", false);
								menuState = MenuState.InPrincipalMenu;
								currentButtonSelected = 1;
								ChangeSelectedButton(true);
								return;
							case 2:
								gameData.GameMode = GameModeStateEnum.Special_Mode;
								sceneName = sceneSelectionGrandSlam;
								break;
							case 3:
								gameData.GameMode = GameModeStateEnum.Classic_Mode;
								sceneName = sceneSelectionClassic;
								break;
							case 4:
								gameData.GameMode = GameModeStateEnum.Volley_Mode;
								sceneName = sceneSelectionVolley;
								break;
							case 5:
								gameData.GameMode = GameModeStateEnum.Bomb_Mode;
								sceneName = sceneSelectionBomb;
								break;
							case 6:
								gameData.GameMode = GameModeStateEnum.Flappy_Mode;
								sceneName = sceneSelectionFlappy;
								break;
						}
						Camera.main.GetComponent<Animator>().SetBool("canTransiToSelect", true);
						timeTransition = 999;
						menuState = MenuState.InTransition;
						StartCoroutine(GoToOtherScene(sceneName));
						break;
				}
			}
		}
	}

	public void ChangeSelectedButton(bool isGoingUp)
	{
		AkSoundEngine.PostEvent(clickIn.Id, this.gameObject);


		OnTransition = true;

		if (isGoingUp)
		{
			switch (menuState)
			{
				case MenuState.InPrincipalMenu:

					if (currentButtonSelected == 1)
						currentButtonSelected = 3;
					else
						currentButtonSelected--;

					switch (currentButtonSelected)
					{
						case 1:
							currentSelectButton = principalButtons[0];
							lastSelectButton = principalButtons[1];
							break;
						case 2:
							currentSelectButton = principalButtons[1];
							lastSelectButton = principalButtons[2];
							break;
						case 3:
							currentSelectButton = principalButtons[2];
							lastSelectButton = principalButtons[0];
							break;
					}
					break;
				case MenuState.InModeMenu:

					if (currentButtonSelected == 1)
						currentButtonSelected = 6;
					else
						currentButtonSelected--;

					switch (currentButtonSelected)
					{
						case 1:
							currentSelectButton = modeButtons[0];
							lastSelectButton = modeButtons[1];
							break;
						case 2:
							currentSelectButton = modeButtons[1];
							lastSelectButton = modeButtons[2];
							break;
						case 3:
							currentSelectButton = modeButtons[2];
							lastSelectButton = modeButtons[3];
							break;
						case 4:
							currentSelectButton = modeButtons[3];
							lastSelectButton = modeButtons[4];
							break;
						case 5:
							currentSelectButton = modeButtons[4];
							lastSelectButton = modeButtons[5];
							break;
						case 6:
							currentSelectButton = modeButtons[5];
							lastSelectButton = modeButtons[0];
							break;
					}
					break;
			}
		}
		else
		{

			switch (menuState)
			{
				case MenuState.InPrincipalMenu:

					if (currentButtonSelected == 3)
						currentButtonSelected = 1;
					else
						currentButtonSelected++;

					switch (currentButtonSelected)
					{
						case 1:
							currentSelectButton = principalButtons[0];
							lastSelectButton = principalButtons[2];
							break;
						case 2:
							currentSelectButton = principalButtons[1];
							lastSelectButton = principalButtons[0];
							break;
						case 3:
							currentSelectButton = principalButtons[2];
							lastSelectButton = principalButtons[1];
							break;
					}
					break;

				case MenuState.InModeMenu:

					if (currentButtonSelected == 6)
						currentButtonSelected = 1;
					else
						currentButtonSelected++;

					switch (currentButtonSelected)
					{
						case 1:
							currentSelectButton = modeButtons[0];
							lastSelectButton = modeButtons[5];
							break;
						case 2:
							currentSelectButton = modeButtons[1];
							lastSelectButton = modeButtons[0];
							break;
						case 3:
							currentSelectButton = modeButtons[2];
							lastSelectButton = modeButtons[1];
							break;
						case 4:
							currentSelectButton = modeButtons[3];
							lastSelectButton = modeButtons[2];
							break;
						case 5:
							currentSelectButton = modeButtons[4];
							lastSelectButton = modeButtons[3];
							break;
						case 6:
							currentSelectButton = modeButtons[5];
							lastSelectButton = modeButtons[4];
							break;
					}
					break;
			}
		}
		currentSelectButton.SetActive(true);
		lastSelectButton.SetActive(false);
		timeTransition = 0.5f;
		//EventSystem.current.SetSelectedGameObject(currentSelectButton);
	}

    public void Options()
	{
		controlMapper.Open();

		currentSelectButton.SetActive(false);
		currentSelectButton = null;
		//clear selected object
		//EventSystem.current.SetSelectedGameObject(null);
	}

	public void CloseOptions()
	{
		OnTransition = true;
		currentButtonSelected = 1;
		currentSelectButton = principalButtons[principalButtons.Count - 2];
		//EventSystem.current.SetSelectedGameObject(currentSelectButton);
		timeTransition = 0.2f;
	}

	private IEnumerator GoToOtherScene(string level)
	{
		//Debug.Log("Ta mère l'anjanath");
		yield return new WaitForSeconds(1.3f);
		UnityEngine.SceneManagement.SceneManager.LoadScene(level);
	}
}