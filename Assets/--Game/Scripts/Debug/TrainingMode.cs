using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Menu
{
	public class TrainingMode : MonoBehaviour, IControllable
	{
		[SerializeField]
		InputController inputController;
		[SerializeField]
		MenuButtonListController listEntry;


		[Title("Scripts")]
		[SerializeField]
		BattleManager battleManager;
		[SerializeField]
		DebugRegisterInput registerInput;
		[SerializeField]
		DebugDummyBehavior dummyBehavior;

		[Title("Parameter")]
		[SerializeField]
		Vector2 timeScaleInterval;
		[SerializeField]
		Vector2 percentageInterval;




		float timeScale = 1f;
		int percentage = 0;

		int characterToRecordID = 1;

		int behavior = 0;

		IControllable character = null;
		bool menuOn = false;




		[Title("UI")]
		[SerializeField]
		GameObject menuUI;
		[SerializeField]
		GameObject selection;

		[Button]
		private void UpdateComponents()
		{
			battleManager = FindObjectOfType<BattleManager>();
			inputController = FindObjectOfType<InputController>();
		}






		private void Start()
		{
			timeScale = 1f;
		}



		private void Update()
		{
			if (menuOn == true)
				return;

			if(inputController.playerInputs[0].CheckAction(0, InputConst.Pause) && menuOn == false)
			{
				inputController.playerInputs[0].inputActions[0].timeValue = 0;

				character = inputController.controllable[0];
				inputController.controllable[0] = this;
				ShowMenu();
			}
			else if (inputController.playerInputs[0].CheckAction(0, InputConst.Back) && menuOn == false)
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













		bool inputDown = false;
		public void UpdateControl(int id, Input_Info input)
		{
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
			else if (input.CheckAction(id, InputConst.Pause) == true)
			{
				inputController.playerInputs[0].inputActions[0].timeValue = 0;
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
			inputController.controllable[0] = character;
			character = null;
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
				case 2: // Dummy Behavior
					behavior += 1 * direction;
					behavior = Mathf.Clamp(behavior, 0, System.Enum.GetValues(typeof(DummyBehavior)).Length-1);
					break;
			}
			DrawOptions();
		}

		private void ValidateOptions()
		{
			// TimeScale
			Time.timeScale = timeScale;

			// Percentage
			for (int i = 0; i < battleManager.characterAlive.Count; i++)
			{
				battleManager.characterAlive[i].Stats.LifePercentage = percentage;
			}

			Debug.Log("Allooooooooooooo");
			// Dummy Behavior
			dummyBehavior.SetBehaviorToCharacter(1, (DummyBehavior)behavior);
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

		private void DrawOptions()
		{
			listEntry.ListItem[0].DrawSubText(timeScale.ToString());
			listEntry.ListItem[1].DrawSubText(percentage.ToString());
			listEntry.ListItem[2].DrawSubText(((DummyBehavior) behavior).ToString());
		}


	}
}

