using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{

	public class MenuTutorials : MenuList, IControllable
	{

		[SerializeField]
		InputController inputController;

		[Title("SubMenu")]
		[SerializeField]
		MenuList[] menu;


		[Title("UI")]
		[SerializeField]
		TextMeshProUGUI[] textDescriptions;

		[Title("Feedbacks")]
		[SerializeField]
		Animator animatorMenu;
		[SerializeField]
		Animator animatorBackground;

		[SerializeField]
		Animator animatorPanelButtonBackground;
		[SerializeField]
		Animator animatorDescription;

		int previousID = 0;


		public override void InitializeMenu()
		{
			for (int i = 0; i < menu.Length; i++)
			{
				menu[i].OnEnd += BackToMenu;
			}
			SelectEntry(0);
			ShowMenu();
		}


		protected override void ValidateEntry(int id)
		{
			inputController.controllable[0] = menu[id];
			menu[id].InitializeMenu();
			animatorBackground.SetBool("Transition", true);
			HideMenu();
		}

		protected override void SelectEntry(int id)
		{
			textDescriptions[previousID].gameObject.SetActive(false);
			textDescriptions[id].gameObject.SetActive(true);
			previousID = listEntry.IndexSelection;

			animatorPanelButtonBackground.SetTrigger("Feedback");
			animatorDescription.SetTrigger("Feedback");
		}


		protected override void QuitMenu()
		{
			base.QuitMenu();
			HideMenu();
			inputController.controllable[0] = null;
			UnityEngine.SceneManagement.SceneManager.LoadScene("GP_Menu");
		}



		private void BackToMenu()
		{
			inputController.controllable[0] = this;
			animatorBackground.SetBool("Transition", false);
			ShowMenu();
		}


		private void ShowMenu()
		{
			animatorMenu.SetBool("Appear", true);
		}

		private void HideMenu()
		{
			animatorMenu.SetBool("Appear", false);
		}





		// Limite ça peut rentrer dans un autre script
		[Title("Percentage")]
		[SerializeField]
		TextMeshProUGUI[] textPercentage;
		[SerializeField]
		SODatabase_Mission[] databaseMissions;

		private void Awake()
		{
			for (int i = 0; i < databaseMissions.Length; i++)
			{
				textPercentage[i].text = GetPercentageUnlocked(databaseMissions[i]) + "%";
			}
			
		}

		private int GetPercentageUnlocked(SODatabase<TrialsModeData> database)
		{
			int max = database.Database.Count;
			int got = 0;
			for (int i = 0; i < database.Database.Count; i++)
			{
				if (database.GetUnlocked(i))
					got++;
			}
			return (int)((got / (float)max) * 100);
		}

	}
}
