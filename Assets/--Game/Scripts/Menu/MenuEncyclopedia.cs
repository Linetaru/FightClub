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

	public class MenuEncyclopedia : MonoBehaviour, IControllable
	{
		[SerializeField]
		MenuButtonListController listEntry;
		[SerializeField]
		MenuButtonListController categoryEntry;

		[SerializeField]
		Encyclopedia[] encyclopediaDatabase;


		[Title("UI")]
		[SerializeField]
		TextMeshProUGUI textDescription;


		private void Start()
		{
			DrawEncyclopedia(encyclopediaDatabase[0]);
		}

		public void DrawEncyclopedia(Encyclopedia encyclopedia)
		{
			for (int i = 0; i < encyclopedia.Lexiques.Length; i++)
			{
				listEntry.DrawItemList(i, null, encyclopedia.Lexiques[i].EntryTitle);
			}
			SelectEntry(0);
			listEntry.SelectIndex(0);
			listEntry.SetItemCount(encyclopedia.Lexiques.Length);
		}



		public void UpdateControl(int id, Input_Info input)
		{
			if (listEntry.InputList(input) == true) // On s'est déplacé dans la liste
			{
				SelectEntry(listEntry.IndexSelection);
			}
			else if (categoryEntry.InputListHorizontal(input) == true)
			{
				NextEncyclopedia();
			}
			else if (input.CheckAction(id, InputConst.Return) == true)
			{
				QuitMenu();
			}
		}




		public void NextEncyclopedia()
		{
			SelectEntry(0);
			listEntry.SelectIndex(0);
			DrawEncyclopedia(encyclopediaDatabase[categoryEntry.IndexSelection]);
		}

		public void SelectEntry(int id)
		{
			textDescription.text = encyclopediaDatabase[categoryEntry.IndexSelection].Lexiques[id].EntryText;
		}


		public void QuitMenu()
		{

		}
	}
}
