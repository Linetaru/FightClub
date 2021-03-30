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
		DebugTimeScale debugTimeScale;

		[Title("Parameter")]
		[SerializeField]
		Vector2 percentageValue;

		int basePercentage = 0;
		float timeScale = 0f;

		IControllable character = null;
		bool menuOn = false;




		[Title("UI")]
		[SerializeField]
		GameObject menuUI;

		[Button]
		private void UpdateComponents()
		{
			battleManager = FindObjectOfType<BattleManager>();
			inputController = FindObjectOfType<InputController>();
		}


		private void Update()
		{
			if(inputController.playerInputs[0].CheckAction(0, InputConst.Pause) && menuOn == false)
			{
				inputController.playerInputs[0].inputActions[0].timeValue = 0;

				character = inputController.controllable[0];
				inputController.controllable[0] = this;
				ShowMenu();
			}
			else if (inputController.playerInputs[0].CheckAction(0, InputConst.DownTaunt) && menuOn == false)
			{
				battleManager.ResetPlayer();
			}
		}

		public void UpdateControl(int id, Input_Info input)
		{
			if (listEntry.InputList(input) == true) // On s'est déplacé dans la liste
			{
				SelectTrainingOption(listEntry.IndexSelection);
			}
			/*else if (categoryEntry.InputListHorizontal(input) == true)
			{
				NextEncyclopedia();
			}*/
			else if (input.CheckAction(id, InputConst.Return) == true)
			{
				QuitMenu();
			}
		}

		public void SelectTrainingOption(int id)
		{
			
		}

		public void QuitMenu()
		{
			inputController.controllable[0] = character;
			character = null;
			HideMenu();
		}







		private void ShowMenu()
		{
			menuOn = true;
			menuUI.SetActive(true);
		}

		private void HideMenu()
		{
			menuOn = false;
			menuUI.SetActive(false);
		}



	}
}

