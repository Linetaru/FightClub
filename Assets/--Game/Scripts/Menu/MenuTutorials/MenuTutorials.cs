using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{

	public class MenuTutorials : MonoBehaviour, IControllable
	{
		[SerializeField]
		MenuButtonListController listEntry;

		[Title("UI")]
		[SerializeField]
		TextMeshProUGUI[] textDescriptions;

		[Title("Feedbacks")]
		[SerializeField]
		Animator animatorBackground;
		[SerializeField]
		Animator animatorPanelButtonBackground;
		[SerializeField]
		Animator animatorDescription;

		int previousID = 0;

		private void Start()
		{
			SelectEntry(0);
		}

		public void UpdateControl(int id, Input_Info input)
		{
			if (listEntry.InputList(input) == true) // On s'est déplacé dans la liste
			{
				SelectEntry(listEntry.IndexSelection);

			}
			else if (input.CheckAction(id, InputConst.Return) == true)
			{
				QuitMenu();
			}
		}



		public void SelectEntry(int id)
		{
			textDescriptions[previousID].gameObject.SetActive(false);
			textDescriptions[id].gameObject.SetActive(true);
			previousID = listEntry.IndexSelection;

			animatorPanelButtonBackground.SetTrigger("Feedback");
			animatorDescription.SetTrigger("Feedback");
		}


		public void QuitMenu()
		{

		}
	}
}
