using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TrialsMode : MonoBehaviour
{

	[Title("Data")]
	[SerializeField]
	TrialsModeData trialsData;



	[Title("Logic")]
	[SerializeField]
	BattleManager battleManager;
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
	[SerializeField]
	Animator animatorRespawn;

	int textIndex = 0;
	int comboIndex = 0;
	int nbOfTry = 0;
	bool success = false;
	List<MissionModePanel> missionModePanels;

	CharacterBase player;
	CharacterBase dummy;
	AIBehavior aiBehavior;

	private void Awake()
	{
		textbox.OnTextEnd += NextText;
	}

	public void InitializeCharacter(CharacterBase character)
	{
		if (character.PlayerID == 0)
		{
			player = character;
		}
		else if (character.PlayerID == 1)
		{
			dummy = character;

			InitializeTrial();
			InitializeFailConditions();
		}
	}


	public void InitializeTrial(TrialsModeData newTrials)
	{
		success = false;
		textIndex = 0;
		comboIndex = 0;
		nbOfTry = 0;
		trialsData = newTrials;

		for (int i = 0; i < missionModePanels.Count; i++)
		{
			Destroy(missionModePanels[i].gameObject);
		}
		missionModePanels.Clear();

		// On reset le behavior pour que Initialize Trial fasse ce qu'il faut
		if(aiBehavior != null)
		{
			inputController.controllable[dummy.ControllerID] = dummy;
			Destroy(aiBehavior.gameObject);
			aiBehavior = null;
		}

		InitializeTrial();
		InitializeFailConditions();
	}

	public void InitializeTrial()
	{
		trialsData.Missions[comboIndex].InitializeCondition(player, dummy);
		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].Stats.LifePercentage = trialsData.EnemyPercentage;
		}

		// AI
		if (trialsData.DummyBehavior != null)
		{
			aiBehavior = Instantiate(trialsData.DummyBehavior, dummy.transform);
			aiBehavior.SetCharacter(dummy, inputController);
		}

		// UI
		missionModePanels = new List<MissionModePanel>(trialsData.Missions.Count);
		for (int i = 0; i < trialsData.Missions.Count; i++)
		{
			missionModePanels.Add(Instantiate(missionModePanel, parentMissionMode));
			missionModePanels[i].gameObject.SetActive(true);
			missionModePanels[i].DrawItem(trialsData.ComboNotes[i]);
		}

		// Text
		if (trialsData.TextboxStart.Count != 0)
		{
			textIndex = 0;
			textbox.DrawTextbox(trialsData.TextboxStart[textIndex]);
			// On échange le player et la textbox
			inputControllerEmpty.controllable = player;
			inputController.controllable[player.ControllerID] = textbox;
		}
	}



	private void InitializeFailConditions()
	{
		for (int i = 0; i < trialsData.FailConditions.Count; i++)
		{
			trialsData.FailConditions[i].InitializeCondition(player, dummy);
		}
	}


	private void EndFailConditions()
	{
		for (int i = 0; i < trialsData.FailConditions.Count; i++)
		{
			trialsData.FailConditions[i].EndCondition(player, dummy);
		}
	}









	// Update is called once per frame
	void Update()
	{
		if (success == true)
			return;

		// Missions
		if(trialsData.Missions[comboIndex].UpdateCondition(player, dummy) == true)
		{
			missionModePanels[comboIndex].ValidateButton();
			trialsData.Missions[comboIndex].EndCondition(player, dummy);
			comboIndex += 1;
			if (comboIndex == trialsData.Missions.Count)
			{
				SuccessTrial();
			}
			else
			{
				trialsData.Missions[comboIndex].InitializeCondition(player, dummy);
			}
		}

		// Fail
		for (int i = 0; i < trialsData.FailConditions.Count; i++)
		{
			if (trialsData.FailConditions[i].UpdateCondition(player, dummy) == true)
			{
				ResetTrial();
			}
		}

	}


	public void ResetTrial()
	{
		if (success == true)
			return;
		animatorRespawn.SetTrigger("Feedback");
		trialsData.Missions[comboIndex].EndCondition(player, dummy);
		EndFailConditions();
		comboIndex = 0;
		battleManager.ResetPlayer();
		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].Stats.LifePercentage = trialsData.EnemyPercentage;
		}
		trialsData.Missions[comboIndex].InitializeCondition(player, dummy);
		InitializeFailConditions();

		if (aiBehavior != null)
			aiBehavior.ResetBehavior();

		// UI
		for (int i = 0; i < missionModePanels.Count; i++)
		{
			missionModePanels[i].ResetButton();
		}
	}




	public void SuccessTrial()
	{
		success = true;
		animatorSuccess.SetActive(true);
		EndFailConditions();

		if (aiBehavior != null)
			aiBehavior.StopBehavior();

		// On échange le player et la textbox
		inputControllerEmpty.controllable = player;
		inputController.controllable[player.ControllerID] = textbox;
		StartCoroutine(WaitSuccess());

	}

	private IEnumerator WaitSuccess()
	{
		yield return new WaitForSeconds(0.5f);

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
			if(textIndex >= trialsData.TextboxStart.Count)
			{
				inputControllerEmpty.controllable = null;
				inputController.controllable[player.ControllerID] = player;
				if (aiBehavior != null)
					aiBehavior.StartBehavior();
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

	public void EndTrial()
	{
		if(trialsData.NextTrial != null)
		{
			animatorSuccess.gameObject.SetActive(false);
			InitializeTrial(trialsData.NextTrial);
			animatorRespawn.SetTrigger("Feedback");
			battleManager.ResetPlayer();
			for (int i = 0; i < battleManager.characterAlive.Count; i++)
			{
				battleManager.characterAlive[i].Stats.LifePercentage = trialsData.EnemyPercentage;
			}
		}

	}


	private void OnDestroy()
	{
		textbox.OnTextEnd -= NextText;
	}



}
