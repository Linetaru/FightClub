using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TrialsMode : MonoBehaviour
{

	[Title("Data")]
	[SerializeField]
	[Expandable]
	GameModeSettingsMission settingsMission;
	[SerializeField]
	CurrencyData currency;

	TrialsModeData trialsData;

	[Title("Logic")]
	//[SerializeField]
	BattleManager battleManager;
	//[SerializeField]
	InputController inputController;
	//[SerializeField]
	//InputControllerEmpty inputControllerEmpty;

	[SerializeField]
	Textbox textbox;
	[SerializeField]
	Transform[] spawnPoints;


	[Title("UI")]
	[SerializeField]
	MissionModePanel missionModePanel;
	[SerializeField]
	Transform parentMissionMode;

	[SerializeField]
	GameObject animatorSuccess;
	[SerializeField]
	GameObject animatorFailed;
	[SerializeField]
	Animator animatorRespawn;
	[SerializeField]
	List<Animator> animatorTries;

	int textIndex = -1;
	int comboIndex = 0;
	int nbOfTry = 0;
	bool success = false;
	bool pauseFailed = false;
	List<MissionModePanel> missionModePanels;

	CharacterBase player;
	CharacterBase dummy;
	AIBehavior aiBehavior;

	private void Awake()
	{
		trialsData = settingsMission.TrialsData;
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
			battleManager = BattleManager.Instance;
			inputController = battleManager.inputController;
			battleManager.aIController.RemoveBehavior(dummy); // on remove le behavior par défaut pour mettre le notre
			InitializeTrial();
		}
	}


	public void InitializeTrial(TrialsModeData newTrials)
	{
		success = false;
		textIndex = 0;
		comboIndex = 0;
		ResetNbOfTry();
		trialsData = newTrials;

		for (int i = 0; i < missionModePanels.Count; i++)
		{
			Destroy(missionModePanels[i].gameObject);
		}
		missionModePanels.Clear();

		// On reset le behavior pour que Initialize Trial fasse ce qu'il faut
		if(aiBehavior != null)
		{
			battleManager.aIController.AIBehaviors.Remove(aiBehavior);
			Destroy(aiBehavior.gameObject);
			aiBehavior = null;
		}

		InitializeTrial();
	}

	public void InitializeTrial()
	{
		player.transform.position = spawnPoints[(int)trialsData.SpawnPlayer].position;
		player.Movement.Direction = (int)spawnPoints[(int)trialsData.SpawnPlayer].transform.localScale.x;
		dummy.transform.position = spawnPoints[(int)trialsData.SpawnEnemy].position;
		dummy.Movement.Direction = (int)spawnPoints[(int)trialsData.SpawnEnemy].transform.localScale.x;


		trialsData.Missions[comboIndex].InitializeCondition(player, dummy);
		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].Stats.LifePercentage = trialsData.EnemyPercentage;
			battleManager.characterAlive[i].PowerGauge.CurrentPower = trialsData.GaugeNumber * 20;
		}

		// AI
		if (trialsData.DummyBehavior != null)
		{
			aiBehavior = Instantiate(trialsData.DummyBehavior, dummy.transform);
			aiBehavior.SetCharacter(dummy, inputController);
			battleManager.aIController.AIBehaviors.Add(aiBehavior);
		}
		else
		{
			aiBehavior = battleManager.aIController.CreateDefaultBehavior(dummy, inputController);
		}

		// UI
		missionModePanels = new List<MissionModePanel>(trialsData.Missions.Count);
		for (int i = 0; i < trialsData.Missions.Count; i++)
		{
			missionModePanels.Add(Instantiate(missionModePanel, parentMissionMode));
			missionModePanels[i].gameObject.SetActive(true);
			missionModePanels[i].DrawItem(trialsData.ComboNotes[i]);
		}
		for (int i = 0; i < trialsData.NumberToSuccess; i++)
		{
			animatorTries[i].gameObject.SetActive(true);
		}
		for (int i = trialsData.NumberToSuccess; i < animatorTries.Count; i++)
		{
			animatorTries[i].gameObject.SetActive(false);
		}

		// Text
		if (trialsData.TextboxStart.Count != 0)
		{
			textIndex = 0;
			textbox.DrawTextbox(trialsData.TextboxStart[textIndex]);
			// On échange le player et la textbox
			battleManager.SetMenuControllable(textbox);
			/*inputControllerEmpty.controllable = player;
			inputController.controllable[player.ControllerID] = textbox;*/
		}
		else
		{
			InitializeFailConditions();
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



	// Quand on appuies sur pause on reset
	public void ForceFail()
	{
		if (pauseFailed == true)
			return;
		if (success == true)
			return;
		if (textIndex > -1)
			return;
		StartCoroutine(FailCoroutine());
	}




	// Update is called once per frame
	void Update()
	{
		if (success == true)
			return;
		if (pauseFailed == true)
			return;
		if (battleManager.GamePaused == true)
			return;

		// Missions
		if (trialsData.Missions[comboIndex].UpdateCondition(player, dummy) == true)
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
				StartCoroutine(FailCoroutine());
			}
		}

	}

	private IEnumerator FailCoroutine()
	{
		if (success == true)
			yield break;
		pauseFailed = true;
		animatorFailed.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		animatorFailed.gameObject.SetActive(false);
		ResetTrial();
		ResetNbOfTry();
		pauseFailed = false;
	}

	public void ResetTrial()
	{
		if (success == true)
			return;
		animatorRespawn.SetTrigger("Feedback");
		if(comboIndex < trialsData.Missions.Count)
			trialsData.Missions[comboIndex].EndCondition(player, dummy);
		EndFailConditions();
		comboIndex = 0;
		battleManager.ResetPlayer();
		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].Stats.LifePercentage = trialsData.EnemyPercentage;
			battleManager.characterAlive[i].PowerGauge.CurrentPower = 20 * trialsData.GaugeNumber;
		}
		player.transform.position = spawnPoints[(int)trialsData.SpawnPlayer].position;
		player.Movement.Direction = (int)spawnPoints[(int)trialsData.SpawnPlayer].transform.localScale.x;
		dummy.transform.position = spawnPoints[(int)trialsData.SpawnEnemy].position;
		dummy.Movement.Direction = (int)spawnPoints[(int)trialsData.SpawnEnemy].transform.localScale.x;
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

	public void ResetNbOfTry()
	{
		nbOfTry = 0;
		for (int i = 0; i < animatorTries.Count; i++)
		{
			animatorTries[i].SetTrigger("Reset");
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
		BattleManager.Instance.SetMenuControllable(textbox);
		/*inputControllerEmpty.controllable = player;
		inputController.controllable[player.ControllerID] = textbox;*/
		StartCoroutine(WaitSuccess());

	}

	private IEnumerator WaitSuccess()
	{
		animatorTries[nbOfTry].SetTrigger("Validate");
		nbOfTry += 1;

		yield return new WaitForSeconds(1f);

		if(nbOfTry >= trialsData.NumberToSuccess)
		{
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
		else
		{
			yield return new WaitForSeconds(0.5f);
			success = false;

			BattleManager.Instance.SetBattleControllable();
			/*inputControllerEmpty.controllable = null;
			inputController.controllable[player.ControllerID] = player;*/

			InitializeFailConditions();
			ResetTrial();
			animatorSuccess.SetActive(false);
		}

	}




	public void NextText()
	{
		textIndex += 1;
		if (success == false) // C'est le texte du début 
		{
			if(textIndex >= trialsData.TextboxStart.Count)
			{
				textIndex = -1;
				battleManager.SetBattleControllable();
				/*inputControllerEmpty.controllable = null;
				inputController.controllable[player.ControllerID] = player;*/
				if (aiBehavior != null)
					aiBehavior.StartBehavior();
				InitializeFailConditions();
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
				textIndex = -1;
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
		// Unlock & get money

		// normalement c'est uniquement null si on spawn direct sur la scene trials Mode
		if (settingsMission.TrialsDatabase != null)
		{
			if (settingsMission.TrialsDatabase.GetUnlocked(trialsData.ToString()) == false)
			{
				currency.AddMoney(trialsData.MoneyReward);
				settingsMission.TrialsDatabase.SetUnlocked(trialsData, true);
				if (SaveManager.Instance != null)
				{
					SaveManager.Instance.SaveFile(settingsMission.TrialsDatabase);
					SaveManager.Instance.SaveFile(currency);
				}
			}
		}





		if (trialsData.NextTrial != null)
		{
			if(trialsData.NextTrial.StageName != UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
			{
				settingsMission.TrialsData = trialsData.NextTrial;
				UnityEngine.SceneManagement.SceneManager.LoadScene(trialsData.NextTrial.StageName);
				return;
			}
			animatorSuccess.gameObject.SetActive(false);
			animatorRespawn.SetTrigger("Feedback");
			battleManager.ResetPlayer();
			for (int i = 0; i < battleManager.characterAlive.Count; i++)
			{
				battleManager.characterAlive[i].Stats.LifePercentage = trialsData.EnemyPercentage;
				battleManager.characterAlive[i].PowerGauge.CurrentPower = trialsData.GaugeNumber * 20;
			}
			battleManager.SetBattleControllable();
			/*inputControllerEmpty.controllable = null;
			inputController.controllable[player.ControllerID] = player;*/
			InitializeTrial(trialsData.NextTrial);
		}
		else
		{
			// Back to menu
		}

	}


	private void OnDestroy()
	{
		if (battleManager.GamePaused == true)
			EndFailConditions();
		textbox.OnTextEnd -= NextText;

	}



}
