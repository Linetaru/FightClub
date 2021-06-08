using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Menu
{
	public class TrainingMode : GameMode, IControllable
	{

		[SerializeField]
		MenuButtonListController listEntry = null;


		[Title("Scripts")]
		[SerializeField]
		DebugRegisterInput registerInput = null;
		[SerializeField]
		DebugDummyBehavior dummyBehavior = null;

		[Title("Parameter")]
		[SerializeField]
		Vector2 timeScaleInterval;
		[SerializeField]
		Vector2 percentageInterval;
		[SerializeField]
		Vector2 powerGaugeInterval;


		BattleManager battleManager = null;
		InputController inputController = null;
		int characterPauseID = 0;

		float timeScale = 1f;
		int percentage = 0;
		int powerGauge = 4;
		bool guardBreakEnemy = false;
		int behavior = 0;
		bool tech = false;

		bool displayInfos = false;
		bool displayInput = false;

		bool menuOn = false;




		[Title("UI")]
		[SerializeField]
		GameObject menuUI;
		[SerializeField]
		GameObject selection;




		public override void InitializeMode(BattleManager battleManager)
		{
			timeScale = 1f;
			this.battleManager = battleManager;
			inputController = battleManager.inputController;
		}


		private void Update()
		{
			if (menuOn == true)
				return;
			if (battleManager.GamePaused)
				return;

			if (inputController.playerInputs[0].CheckAction(0, InputConst.Back) && menuOn == false)
			{
				if (registerInput.registerInput == true)
					registerInput.StopRegisterInput();
				if (registerInput.playInput == true)
					registerInput.StopPlayInput();
				battleManager.ResetPlayer();
				ValidateOptions();
			}
			else if (inputController.playerInputs[0].CheckAction(0, InputConst.DownTaunt))
			{
				inputController.playerInputs[0].inputActions[0].timeValue = 0;
				if (registerInput.registerInput == true)
					registerInput.StopRegisterInput();
				else
					registerInput.StartRegisterInput();

			}
			else if (inputController.playerInputs[0].CheckAction(0, InputConst.UpTaunt))
			{
				inputController.playerInputs[0].inputActions[0].timeValue = 0;
				registerInput.StartPlayInput();
			}
		}





		public void OpenMenu(int id)
		{
			if (menuOn)
				return;
			if (battleManager.GamePaused)
				return;
			characterPauseID = id;
			battleManager.SetMenuControllable(this);

			for (int i = 0; i < battleManager.inputController.playerInputs.Length; i++)
			{
				battleManager.inputController.playerInputs[i].inputUiAction = null;
			}
			ShowMenu();
		}







		bool inputDown = false;
		public void UpdateControl(int id, Input_Info input)
		{
			if (id != characterPauseID)
				return;
			if (listEntry.InputList(input) == true) // On s'est déplacé dans la liste
			{
				SelectTrainingOption(listEntry.IndexSelection);
			}
			else if (Mathf.Abs(input.horizontal) > 0.5f && inputDown == false)
			{
				ModifyOptions(listEntry.IndexSelection, (int)Mathf.Sign(input.horizontal));
				inputDown = true;
			}
			else if (Mathf.Abs(input.horizontal) < 0.5f && inputDown == true)
			{
				inputDown = false;
			}
			else if (input.inputUiAction == InputConst.Pause || input.inputUiAction == InputConst.Back)
			{
				input.inputUiAction = null;
				ValidateOptions();
				QuitMenu();
			}
		}

		public void SelectTrainingOption(int id)
		{
			selection.transform.position = listEntry.ListItem[id].transform.position;
		}

		public void QuitMenu()
		{
			battleManager.SetBattleControllable();
			HideMenu();
		}




		private void ModifyOptions(int indexSelection, int direction)
		{
			switch(indexSelection)
			{
				case 0: // TimeScale
					timeScale += 0.1f * direction;
					timeScale = Mathf.Clamp(timeScale, timeScaleInterval.x, timeScaleInterval.y);
					break;
				case 1: // Percentage
					percentage += 10 * direction;
					percentage = (int) Mathf.Clamp(percentage, percentageInterval.x, percentageInterval.y);
					break;
				case 2: // Power Gauge
					powerGauge += 1 * direction;
					powerGauge = (int)Mathf.Clamp(powerGauge, powerGaugeInterval.x, powerGaugeInterval.y);
					break;
				case 3: // Guard Break Enemy
					guardBreakEnemy = !guardBreakEnemy;
					break;
				case 4: // Dummy Behavior
					behavior += 1 * direction;
					behavior = Mathf.Clamp(behavior, 0, System.Enum.GetValues(typeof(DummyBehavior)).Length-1);
					break;
				case 5: // Tech
					tech = !tech;
					break;
				case 6: // Display Infos
					displayInfos = !displayInfos;
					break;
				case 7: // Display Inputs
					displayInput = !displayInput;
					break;
			}
			DrawOptions();
		}

		private void ValidateOptions()
		{
			// TimeScale
			Time.timeScale = timeScale;

			// Percentage && Power Gauge
			for (int i = 0; i < battleManager.characterAlive.Count; i++)
			{
				battleManager.characterAlive[i].Stats.LifePercentage = percentage;
				battleManager.characterAlive[i].PowerGauge.CurrentPower = powerGauge * 20;
			}

			// Dummy Behavior
			dummyBehavior.SetBehaviorToCharacter(1, (DummyBehavior)behavior);
		}


		private void DrawOptions()
		{
			listEntry.ListItem[0].DrawSubText(timeScale.ToString());
			listEntry.ListItem[1].DrawSubText(percentage.ToString());
			listEntry.ListItem[2].DrawSubText(powerGauge.ToString());
			listEntry.ListItem[3].DrawSubText(guardBreakEnemy ? "On" : "Off");
			listEntry.ListItem[4].DrawSubText(((DummyBehavior)behavior).ToString());
			listEntry.ListItem[5].DrawSubText(tech ? "On" : "Off");
			listEntry.ListItem[6].DrawSubText(displayInfos ? "On" : "Off");
			listEntry.ListItem[7].DrawSubText(displayInput ? "On" : "Off");
		}




		private void ShowMenu()
		{
			menuOn = true;
			menuUI.SetActive(true);
			DrawOptions();
		}

		private void HideMenu()
		{
			menuUI.SetActive(false);
			StartCoroutine(WaitOneFrame());
		}

		private IEnumerator WaitOneFrame()
		{
			yield return null;
			menuOn = false;
		}




	}
}

