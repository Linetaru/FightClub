using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired.UI.ControlMapper;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuManager : MonoBehaviour, IControllable
{
	private ControlMapper controlMapper;

	[Header("Panels")]
	public GameObject panelPrincipal;


	[Header("Button Panel Principal")]
	public GameObject firstButton;
	public GameObject secondButton;
	public GameObject thridButton;
	public GameObject fourthButton;


	[Header("Selected Button")]
	[ReadOnly] public GameObject currentSelectButton;
	[ReadOnly] public GameObject lastSelectButton;

	[Header("Level Name")]
	public string level;

	bool OnTransition = false;
	float timeTransition = 0;

	// Start is called before the first frame update
	void Start()
	{
		controlMapper = GameObject.FindObjectOfType<ControlMapper>();

		EventSystem.current.SetSelectedGameObject(firstButton);
		currentSelectButton = firstButton;
		firstButton.transform.DOScale(new Vector3(1.5f, 1.5f, 0), 0.5f);

		controlMapper.onScreenClosed -= CloseOptions;
		controlMapper.onScreenClosed += CloseOptions;
	}

    public void UpdateControl(int ID, Input_Info input_Info)
	{
		if(timeTransition > 0)
        {
			timeTransition -= Time.deltaTime;
		}

		if (input_Info.vertical < -0.75)
			ChangeSelectedButton(false);
		else if (input_Info.vertical > 0.75)
			ChangeSelectedButton(true);

		if (input_Info.inputUiAction == InputConst.Pause)
		{
			input_Info.inputUiAction = null;
			Options();
		}
		else if(input_Info.inputUiAction == InputConst.Interact)
        {
			if(currentSelectButton == thridButton && panelPrincipal.activeSelf && timeTransition <= 0)
            {
				Options();
			}
			else if(currentSelectButton == fourthButton && panelPrincipal.activeSelf && timeTransition <= 0)
            {
				Application.Quit();
			}
        }
		else if (input_Info.inputUiAction == InputConst.Return)
		{
			Debug.Log("Return with Button");
		}

	}

	public void ChangeSelectedButton(bool isGoingUp)
	{
		if (!OnTransition && panelPrincipal.activeSelf)
		{
			OnTransition = true;
			if (isGoingUp)
			{
				if (currentSelectButton == firstButton)
				{
					currentSelectButton = fourthButton;
					lastSelectButton = firstButton;
					EventSystem.current.SetSelectedGameObject(fourthButton);
				}
				else if (currentSelectButton == secondButton)
				{
					currentSelectButton = firstButton;
					lastSelectButton = secondButton;
					EventSystem.current.SetSelectedGameObject(firstButton);
				}
				else if (currentSelectButton == thridButton)
				{
					currentSelectButton = secondButton;
					lastSelectButton = thridButton;
					EventSystem.current.SetSelectedGameObject(secondButton);
				}
				else
				{
					currentSelectButton = thridButton;
					lastSelectButton = fourthButton;
					EventSystem.current.SetSelectedGameObject(thridButton);
				}
			}
			else
			{
				if (currentSelectButton == firstButton)
				{
					currentSelectButton = secondButton;
					lastSelectButton = firstButton;
					EventSystem.current.SetSelectedGameObject(secondButton);
				}
				else if (currentSelectButton == secondButton)
				{
					currentSelectButton = thridButton;
					lastSelectButton = secondButton;
					EventSystem.current.SetSelectedGameObject(thridButton);
				}
				else if (currentSelectButton == thridButton)
				{
					currentSelectButton = fourthButton;
					lastSelectButton = thridButton;
					EventSystem.current.SetSelectedGameObject(fourthButton);
				}
				else
				{
					currentSelectButton = firstButton;
					lastSelectButton = fourthButton;
					EventSystem.current.SetSelectedGameObject(firstButton);

				}
			}
			lastSelectButton.transform.DOScale(new Vector3(1, 1, 0), 0.5f);
			currentSelectButton.transform.DOScale(new Vector3(1.5f, 1.5f, 0), 0.5f).OnComplete(() => OnTransition = false);
		}
	}

    public void Options()
    {
		if (!controlMapper.isOpen)
		{
			controlMapper.Open();
			panelPrincipal.SetActive(false);

			//clear selected object
			EventSystem.current.SetSelectedGameObject(null);
		}
		else
		{
			controlMapper.Close(true);
			panelPrincipal.SetActive(true);
			EventSystem.current.SetSelectedGameObject(firstButton);
		}
	}

	public void CloseOptions()
    {
		panelPrincipal.SetActive(true);
		EventSystem.current.SetSelectedGameObject(firstButton);
		timeTransition = 1f;
	}
}