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


	[Title("UI")]
	[SerializeField]
	MissionModePanel missionModePanel;
	[SerializeField]
	List<MissionModePanel> missionModePanels;
	[SerializeField]
	Transform parentMissionMode;

	[SerializeField]
	GameObject animatorSuccess;


	int comboIndex = 0;
	bool success = false;

	CharacterBase player;
	CharacterBase dummy;

	public void InitializeCharacter(CharacterBase character)
	{
		if (character.PlayerID == 0)
		{
			player = character;
		}
		else if (character.PlayerID == 1)
		{
			dummy = character;
			dummy.OnStateChanged += StateChangedCallback;
			InitializeTrial();
		}
	}



	// Update is called once per frame
	void Update()
	{
		if (success == true)
			return;

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
	}



	public void StateChangedCallback(CharacterState oldState, CharacterState newState)
	{
		if(oldState is CharacterStateKnockback && (newState is CharacterStateIdle || newState is CharacterStateAerial))
		{
			ResetTrial();
		}
	}



	public void InitializeTrial()
	{
		trialsData.Missions[comboIndex].InitializeCondition(player, dummy);
		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].Stats.LifePercentage = trialsData.EnemyPercentage;
		}

		// UI
		missionModePanels = new List<MissionModePanel>(trialsData.Missions.Count);
		for (int i = 0; i < trialsData.Missions.Count; i++)
		{
			missionModePanels.Add(Instantiate(missionModePanel, parentMissionMode));
			missionModePanels[i].gameObject.SetActive(true);
			missionModePanels[i].DrawItem(trialsData.ComboNotes[i]);
		}
	}


	public void SuccessTrial()
	{
		success = true;
		animatorSuccess.SetActive(true);
		Debug.Log("Success");
	}

	public void ResetTrial()
	{
		if (success == true)
			return;
		trialsData.Missions[comboIndex].EndCondition(player, dummy);
		comboIndex = 0;
		battleManager.ResetPlayer();
		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].Stats.LifePercentage = trialsData.EnemyPercentage;
		}
		trialsData.Missions[comboIndex].InitializeCondition(player, dummy);

		for (int i = 0; i < missionModePanels.Count; i++)
		{
			missionModePanels[i].ResetButton();
		}
	}
}
