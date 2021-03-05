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
		Encyclopedia[] encyclopediaDatabase;


		[Title("UI")]
		[SerializeField]
		TextMeshProUGUI textDescription;


		int indexEncyclopedia = 0;
		bool padDown = false;


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
			listEntry.SetItemCount(encyclopedia.Lexiques.Length);
		}

		public void UpdateControl(int id, Input_Info input)
		{
			if (listEntry.InputList(input) == true) // On s'est déplacé dans la liste
				SelectEntry(listEntry.IndexSelection);
			else if (Mathf.Abs(input.horizontal) > 0.9f && padDown == false)
			{
				NextEncyclopedia();
				padDown = true;
			}
			else if (input.CheckAction(id, InputConst.Return) == true)
				QuitMenu();
			else if (Mathf.Abs(input.horizontal) < 0.9f)
			{
				padDown = false;
			}
		}




		public void NextEncyclopedia()
		{
			indexEncyclopedia += 1;
			if (indexEncyclopedia >= encyclopediaDatabase.Length)
				indexEncyclopedia = 0;
			DrawEncyclopedia(encyclopediaDatabase[indexEncyclopedia]);
		}

		public void SelectEntry(int id)
		{
			textDescription.text = encyclopediaDatabase[indexEncyclopedia].Lexiques[id].EntryText;
		}


		public void QuitMenu()
		{

		}
	}
}
