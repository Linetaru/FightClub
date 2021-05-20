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


		[Title("UI")]
		[SerializeField]
		Animator animatorMenu;

		[SerializeField]
		TextMeshProUGUI textDescription;
		[SerializeField]
		Sprite unlockedSprite;



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
		}

		protected override void ValidateEntry(int id)
		{
			base.ValidateEntry(id);

			// Debug test save
			databaseMission.SetUnlocked(id, true);
			SaveManager.Instance.SaveFile();
			InitializeMenu();
		}

		protected override void QuitMenu()
		{
			base.QuitMenu();

			animatorMenu.SetBool("Appear", false);
		}

	}
}
