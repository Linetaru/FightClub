using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TutorialManager1 : MonoBehaviour
{

	[Title("Data")]
	[SerializeField]
	TrialsModeData trialsData;



	[Title("Logic")]
	[SerializeField]
	InputController inputController;
	[SerializeField]
	InputControllerEmpty inputControllerEmpty;

	[SerializeField]
	Textbox textbox;



	[Title("UI")]
	[SerializeField]
	MissionModePanel missionModePanel;
	[SerializeField]
	Transform parentMissionMode;

	[SerializeField]
	GameObject animatorSuccess;

	int textIndex = 0;
	int comboIndex = 0;
	bool success = false;
	List<MissionModePanel> missionModePanels = new List<MissionModePanel>();

	CharacterBase player;

	public void InitializeCharacter(CharacterBase character)
	{
		player = character;
		InitializeTrial();
		InitializeTrial(trialsData);
	}




	public void InitializeTrial()
	{
		textbox.OnTextEnd += NextText;
		if (trialsData.TextboxStart.Count != 0)
		{
			textIndex = 0;
			textbox.DrawTextbox(trialsData.TextboxStart[textIndex]);
			// On échange le player et la textbox
			inputControllerEmpty.controllable = player;
			inputController.controllable[player.ControllerID] = textbox;
		}
	}

	public void InitializeTrial(TrialsModeData trialsModeData)
	{
		trialsData = trialsModeData;
		comboIndex = 0;

		// UI
		for (int i = 0; i < missionModePanels.Count; i++)
		{
			Destroy(missionModePanels[i].gameObject);
		}
		missionModePanels.Clear();
		missionModePanels = new List<MissionModePanel>(trialsData.Missions.Count);
		for (int i = 0; i < trialsData.Missions.Count; i++)
		{
			missionModePanels.Add(Instantiate(missionModePanel, parentMissionMode));
			missionModePanels[i].gameObject.SetActive(true);
			missionModePanels[i].DrawItem(trialsData.ComboNotes[i]);
		}
	}

	public void NextStepTrial()
	{
		missionModePanels[comboIndex].ValidateButton();
		comboIndex += 1;
	}



	public void SuccessTrial()
	{
		success = true;
		animatorSuccess.SetActive(true);

		// On échange le player et la textbox
		inputControllerEmpty.controllable = player;
		inputController.controllable[player.ControllerID] = textbox;

		if (trialsData.TextboxEnd.Count != 0)
		{
			textIndex = 0;
			textbox.DrawTextbox(trialsData.TextboxEnd[textIndex]);
		}
		else
		{
			EndTrial();
		}
	}





	public void NextText()
	{
		textIndex += 1;
		if (success == false) // C'est le texte du début 
		{
			if (textIndex >= trialsData.TextboxStart.Count)
			{
				inputControllerEmpty.controllable = null;
				inputController.controllable[player.ControllerID] = player;
			}
			else
			{
				textbox.DrawTextbox(trialsData.TextboxStart[textIndex]);
			}
		}
		else // C'est le texte de fin
		{
			if (textIndex >= trialsData.TextboxEnd.Count)
			{
				EndTrial();
			}
			else
			{
				textbox.DrawTextbox(trialsData.TextboxEnd[textIndex]);
			}
		}

	}

	private void EndTrial()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialStage2");
	}


	private void OnDestroy()
	{
		textbox.OnTextEnd -= NextText;
	}

}
