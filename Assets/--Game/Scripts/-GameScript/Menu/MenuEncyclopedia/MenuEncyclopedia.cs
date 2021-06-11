using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{
	[System.Serializable]
	public class Encyclopedia
	{
		[SerializeField]
		LexiqueData[] lexiques;
		public LexiqueData[] Lexiques
		{
			get { return lexiques; }
		}

	}

	public class MenuEncyclopedia : MenuList, IControllable
	{
		[SerializeField]
		MenuButtonListController categoryEntry;

		[SerializeField]
		SODatabase_Encyclopedia[] encyclopediaDatabase;


		[Title("UI")]
		[SerializeField]
		TextMeshProUGUI textDescription;


		List<int> idItems = new List<int>();
		bool firstTime = false;


		/*private void Start()
		{
			DrawEncyclopedia(encyclopediaDatabase[0]);
		}*/

		public override void InitializeMenu()
		{
			base.InitializeMenu();
			if (!firstTime)
			{
				DrawEncyclopedia(encyclopediaDatabase[0]);
				firstTime = true;
			}
		}

		public void DrawEncyclopedia(SODatabase_Encyclopedia encyclopedia)
		{
			idItems.Clear();
			for (int i = 0; i < encyclopedia.Database.Count; i++)
			{
				if (encyclopedia.GetUnlocked(i))
				{
					listEntry.DrawItemList(i, null, encyclopedia.Database[i].EntryTitle);
					idItems.Add(i);
				}
			}
			if (idItems.Count == 0)
				return;
			SelectEntry(0);
			listEntry.SelectIndex(0);
			listEntry.SetItemCount(idItems.Count);
		}



		public override void UpdateControl(int id, Input_Info input)
		{
			if (listEntry.InputList(input) == true) // On s'est déplacé dans la liste
			{
				SelectEntry(listEntry.IndexSelection);
			}
			else if (categoryEntry.InputListHorizontal(input) == true)
			{
				NextEncyclopedia();
			}
			else if (input.inputUiAction == InputConst.Return)
			{
				QuitMenu();
			}
		}



		protected override void SelectEntry(int id)
		{
			if (idItems.Count == 0)
				return;
			base.SelectEntry(id);
			textDescription.text = encyclopediaDatabase[categoryEntry.IndexSelection].Database[idItems[id]].EntryText;
		}



		/*protected override void QuitMenu()
		{

		}*/





		public void NextEncyclopedia()
		{
			/*SelectEntry(0);
			listEntry.SelectIndex(0);*/
			DrawEncyclopedia(encyclopediaDatabase[categoryEntry.IndexSelection]);
		}
	}
}
