using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TutorialManager1 : MonoBehaviour
{

	[Title("Data")]
	[SerializeField]
	TrialsModeData trialsData = null;
	[SerializeField]
	GameModeSettingsMission settingsMission = null;

	[Title("Logic")]
	[SerializeField]
	InputController inputController = null;
	[SerializeField]
	InputControllerEmpty inputControllerEmpty = null;

	[SerializeField]
	TrialsButtonDrawer buttonDrawer = null;
	[SerializeField]
	Textbox textbox = null;



	[Title("UI")]
	[SerializeField]
	MissionModePanel missionModePanel = null;
	[SerializeField]
	Transform parentMissionMode = null;

	[SerializeField]
	GameObject animatorSuccess = null;

	[Title("Debug Transition Scene")]
	[SerializeField]
	GameData gameData = null;
	[SerializeField]
	CharacterData bernard = null;

	int textIndex = 0;
	int comboIndex = 0;
	bool success = false;
	List<MissionModePanel> missionModePanels = new List<MissionModePanel>();

	CharacterBase player;

	bool once = false;

	public void InitializeCharacter(CharacterBase character)
	{
		if (once == false)
		{
			player = character;
			InitializeTrial();
			InitializeTrial(trialsData);
			once = true;
		}
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
		string textUI;
		for (int i = 0; i < trialsData.Missions.Count; i++)
		{
			textUI = trialsData.ComboNotes[i];
			missionModePanels.Add(Instantiate(missionModePanel, parentMissionMode));
			missionModePanels[i].gameObject.SetActive(true);
			if (trialsData.TrialsButtonsNote.Count > i)
				textUI = buttonDrawer.AddButtonToText(trialsData.TrialsButtonsNote[i], inputController.playerInputs[player.ControllerID], trialsData.ComboNotes[i]);
			missionModePanels[i].DrawItem(textUI);
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
		Character_Info c = new Character_Info();
		c.ControllerID = -10;
		c.CharacterData = bernard;
		c.CharacterColorID = 3;
		gameData.CharacterInfos.Add(c);

		if (settingsMission.TrialsDatabase != null)
		{
			settingsMission.TrialsDatabase.SetUnlocked(0, true);
			if (SaveManager.Instance != null)
				SaveManager.Instance.SaveFile(settingsMission.TrialsDatabase);
		}

		settingsMission.TrialsData = trialsData.NextTrial;
		Debug.Log(trialsData.NextTrial);
		Debug.Log(settingsMission.TrialsData);

		UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialStage2");

	}


	private void OnDestroy()
	{
		textbox.OnTextEnd -= NextText;
	}

}
