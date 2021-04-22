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
	[Title("Canvas Object")]
	public List<TextMeshProUGUI> startTexts;
	public GameObject playButton;
	public GameObject optionButton;
	public GameObject quitButton;

	[Title("Parameter Variable")]
	[ReadOnly] public bool isStartMenu = true;
	bool OnTransition = false;
	float timeTransition = 0;
	bool CanPulse = true;
	[ReadOnly] public int currentButtonSelected;
	[HideInInspector] public bool canChangeScene;

	[Title("Selected Button")]
	[ReadOnly] public GameObject currentSelectButton;
	[ReadOnly] public GameObject lastSelectButton;

	[Title("ControlMapper")]
	public ControlMapper controlMapper;

	[Title("Level Name")]
	public string level;
	public AK.Wwise.Event clickIn;
	public AK.Wwise.Event pressStart;
	public AK.Wwise.Event mainWhoosh;

	private void Start()
    {
		Pulse();
		controlMapper.Open();
		controlMapper.onScreenClosed -= CloseOptions;
		controlMapper.onScreenClosed += CloseOptions;
		controlMapper.Close(true);
	}

	public void Pulse()
    {
        //if(CanPulse)
        //	foreach (TextMeshProUGUI text in startTexts)
        //		text.transform.DOScale(1f, 0.5f).OnComplete(() => text.transform.DOScale(0.8f, 0.5f).OnComplete(Pulse));
    }

    public void UpdateControl(int ID, Input_Info input_Info)
	{

		if (timeTransition > 0)
		{
			timeTransition -= Time.deltaTime;
		}

		if(canChangeScene)
        {
			GoToOtherScene();
		}

		if (!controlMapper.isOpen && timeTransition <= 0)
		{
			if (!OnTransition && !isStartMenu)
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

			if (input_Info.inputUiAction == InputConst.Pause && isStartMenu)
			{
				AkSoundEngine.PostEvent(pressStart.Id, this.gameObject);
				AkSoundEngine.PostEvent(mainWhoosh.Id, this.gameObject);
				
				isStartMenu = false;
				Camera.main.transform.gameObject.GetComponent<Animator>().enabled = true;
				foreach (TextMeshProUGUI text in startTexts)
					text.transform.gameObject.SetActive(false);
				//EventSystem.current.SetSelectedGameObject(playButton);
				playButton.transform.DOScale(new Vector3(1.5f, 1.5f, 0), 0.2f);
				optionButton.transform.DOScale(new Vector3(1f, 1f, 0), 0.2f);
				currentButtonSelected = 1;
				currentSelectButton = playButton;
				CanPulse = false;
			}

			if (!isStartMenu && input_Info.inputUiAction == InputConst.Interact)
			{
				switch (currentButtonSelected)
				{
					case 1:
						Camera.main.GetComponent<Animator>().SetBool("canTransition", true);
						timeTransition = 999;
						canChangeScene = true;
						break;
					case 2:
						Options();
						break;
					case 3:
						Application.Quit();
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
			if (currentButtonSelected == 1)
				currentButtonSelected = 3;
			else
				currentButtonSelected--;

			switch (currentButtonSelected)
            {
				case 1:
					currentSelectButton = playButton;
					lastSelectButton = optionButton;
					break;
				case 2:
					currentSelectButton = optionButton;
					lastSelectButton = quitButton;
					break;
				case 3:
					currentSelectButton = quitButton;
					lastSelectButton = playButton;
					break;
            }
        }
		else
        {
			if (currentButtonSelected == 3)
				currentButtonSelected = 1;
			else
				currentButtonSelected++;

			switch (currentButtonSelected)
			{
				case 1:
					currentSelectButton = playButton;
					lastSelectButton = quitButton;
					break;
				case 2:
					currentSelectButton = optionButton;
					lastSelectButton = playButton;
					break;
				case 3:
					currentSelectButton = quitButton;
					lastSelectButton = optionButton;
					break;
			}
		}
		currentSelectButton.transform.DOScale(new Vector3(1.5f, 1.5f, 0), 0.5f).OnComplete(() => OnTransition = false);
		lastSelectButton.transform.DOScale(new Vector3(1, 1, 0), 0.5f);
		//EventSystem.current.SetSelectedGameObject(currentSelectButton);
	}

	public void Options()
	{
		controlMapper.Open();

		currentSelectButton.transform.DOScale(new Vector3(1, 1, 0), 0.2f);
		currentSelectButton = null;
		//clear selected object
		//EventSystem.current.SetSelectedGameObject(null);
	}

	public void CloseOptions()
	{
		OnTransition = true;
		optionButton.transform.DOScale(new Vector3(1.5f, 1.5f, 0), 0.2f).OnComplete(() => OnTransition = false);
		currentButtonSelected = 1;
		currentSelectButton = optionButton;
		//EventSystem.current.SetSelectedGameObject(currentSelectButton);
		timeTransition = 0.2f;
	}

	public void GoToOtherScene()
    {
		Debug.Log("Ta mère l'anjanath");
		UnityEngine.SceneManagement.SceneManager.LoadScene(level);
	}
}