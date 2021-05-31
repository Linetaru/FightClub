using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{

	public class MenuListMission : MenuList, IControllable
	{
		[SerializeField]
		SODatabase_Mission databaseMission;


		[Title("Data")]
		[SerializeField]
		GameData gameData;
		[SerializeField]
		GameModeSettingsMission missionSettings;

		[Title("UI")]
		[SerializeField]
		TextMeshProUGUI textDescription;
		[SerializeField]
		Sprite unlockedSprite;


		[Title("Feedbacks")]
		[SerializeField]
		Animator animatorMenu;
		[SerializeField]
		Animator animatorDescription;

		public override void InitializeMenu()
		{
			animatorMenu.gameObject.SetActive(true);
			animatorMenu.SetBool("Appear", true);

			for (int i = 0; i < databaseMission.Database.Count; i++)
			{
				if(databaseMission.GetUnlocked(i) == true)
					listEntry.DrawItemList(i, unlockedSprite, databaseMission.Database[i].TrialsName);
				else
					listEntry.DrawItemList(i, null, databaseMission.Database[i].TrialsName);
			}
			SelectEntry(0);
			listEntry.SelectIndex(0);
			listEntry.SetItemCount(databaseMission.Database.Count);
			base.InitializeMenu();
		}



		protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);
			animatorDescription.SetTrigger("Feedback");
			textDescription.text = databaseMission.Database[id].TrialsDescription;
		}

		protected override void ValidateEntry(int id)
		{
			base.ValidateEntry(id);

			// Debug test save
			/*databaseMission.SetUnlocked(id, true);
			SaveManager.Instance.SaveFile();
			InitializeMenu();*/

			gameData.GameMode = GameModeStateEnum.Classic_Mode;
			gameData.NumberOfLifes = 3;
			gameData.CharacterInfos.Clear();

			if(databaseMission.Database[id].Player != null)
			{
				gameData.CharacterInfos.Add(new Character_Info());
				gameData.CharacterInfos[0].CharacterData = databaseMission.Database[id].Player;
				gameData.CharacterInfos[0].ControllerID = 0;
				gameData.CharacterInfos[0].CharacterColorID = 0;
				gameData.CharacterInfos[0].Team = TeamEnum.First_Team;
			}

			if (databaseMission.Database[id].Dummy != null)
			{
				gameData.CharacterInfos.Add(new Character_Info());
				gameData.CharacterInfos[1].CharacterData = databaseMission.Database[id].Dummy;
				gameData.CharacterInfos[1].ControllerID = 1;
				gameData.CharacterInfos[1].CharacterColorID = 3;
				gameData.CharacterInfos[1].Team = TeamEnum.Second_Team;
			}

			missionSettings.TrialsData = databaseMission.Database[id];
			missionSettings.TrialsDatabase = databaseMission;

			UnityEngine.SceneManagement.SceneManager.LoadScene(databaseMission.Database[id].StageName);

		}

		protected override void QuitMenu()
		{
			base.QuitMenu();

			animatorMenu.SetBool("Appear", false);
		}

	}
}
