using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Menu
{
	public class MenuCharacterSelectionList : MenuList, IControllable
	{
		[SerializeField]
		SODatabase_Character databaseCharacter;
		[SerializeField]
		SODatabase_Mission[] databaseMission;

		[Title("Data")]
		[SerializeField]
		GameData gameData;
		[SerializeField]
		InputController inputController;
		[SerializeField]
		GameModeSettingsMission missionSettings;
		[SerializeField]
		MenuListMission menuMissionSelection;


		[Title("Feedbacks")]
		[SerializeField]
		Transform characterModel;
		[SerializeField]
		Animator animatorBackground;
		[SerializeField]
		Animator animatorMenu;

		List<int> ids = new List<int>();
		List<CharacterModel> model = new List<CharacterModel>();


		private void Awake()
		{
			menuMissionSelection.OnEnd += BackToMenu;
		}


		public override void InitializeMenu()
		{
			ids.Clear();
			animatorMenu.gameObject.SetActive(true);
			animatorMenu.SetBool("Appear", true);

			for (int i = 0; i < databaseCharacter.Database.Count; i++)
			{
				if (databaseCharacter.GetUnlocked(i) == true)
				{
					listEntry.DrawItemList(i, null, databaseCharacter.Database[i].characterName);
					ids.Add(i);
					if (model.Count < ids.Count)
						model.Add(null);
				}
			}
			SelectEntry(0);
			listEntry.SelectIndex(0);
			listEntry.SetItemCount(ids.Count);
			base.InitializeMenu();
		}



		protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);


			for (int i = 0; i < ids.Count; i++)
			{
				if (model[i] != null)
					model[i].gameObject.SetActive(false);
			}

			if (model[ids[id]] == null)
			{
				model[ids[id]] = Instantiate(databaseCharacter.Database[ids[id]].looserModel, characterModel);
				model[ids[id]].SetColor(-1, databaseCharacter.Database[ids[id]].characterMaterials[0]);
			}
			else
			{
				model[ids[id]].gameObject.SetActive(true);
			}
			/*animatorDescription.SetTrigger("Feedback");
			textDescription.text = databaseMission.Database[id].TrialsDescription;*/
		}

		protected override void ValidateEntry(int id)
		{
			base.ValidateEntry(id);
			if (databaseMission.Length <= ids[id])
				return;
			missionSettings.TrialsDatabase = databaseMission[ids[id]];
			inputController.controllable[0] = menuMissionSelection;
			menuMissionSelection.SetDatabase(missionSettings.TrialsDatabase);
			menuMissionSelection.InitializeMenu();
			animatorMenu.SetBool("Appear", false);
			animatorBackground.SetBool("Transition2", true);

		}

		protected override void QuitMenu()
		{
			base.QuitMenu();
			animatorMenu.SetBool("Appear", false);
			for (int i = 0; i < ids.Count; i++)
			{
				if (model[i] != null)
					model[i].gameObject.SetActive(false);
			}
		}

		private void BackToMenu()
		{
			inputController.controllable[0] = this;
			animatorMenu.gameObject.SetActive(true);
			animatorMenu.SetBool("Appear", true);
			animatorBackground.SetBool("Transition2", false);
		}
	}
}
