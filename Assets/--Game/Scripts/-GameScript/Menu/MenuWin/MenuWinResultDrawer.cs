using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Menu
{
	public class MenuWinResultDrawer : MonoBehaviour
	{

		[SerializeField]
		Animator animator;


		[SerializeField]
		TextMeshProUGUI textPosition;
		[SerializeField]
		TextMeshProUGUI textControllerID;

		[SerializeField]
		TextMeshProUGUI textCharacterName;
		[SerializeField]
		TextMeshProUGUI textKilled;
		[SerializeField]
		TextMeshProUGUI textKiller;
		[SerializeField]
		TextMeshProUGUI textParry;
		[SerializeField]
		TextMeshProUGUI textBestAttack;

		[SerializeField]
		TextMeshProUGUI feedback;


		public void DrawResult(int position, int controllerID)
		{

			switch(position)
			{
				case 1:
					textPosition.text = "1st";
					break;
				case 2:
					textPosition.text = "2nd";
					break;
				case 3:
					textPosition.text = "3rd";
					break;
				case 4:
					textPosition.text = "4th";
					break;
			}
			textControllerID.text = controllerID + "P";
			this.gameObject.SetActive(true);
		}

		public void DrawCharacterName(string playerName)
        {
			textCharacterName.text = playerName;
		}

		public void DrawParry(int nbParry)
		{
			textParry.text = nbParry.ToString();
		}

		public void DrawKilled(List<CharacterBase> characterKilled)
		{
			textKilled.text = characterKilled.Count.ToString();
			textKilled.text += "<size=24> (";
			for (int i = 0; i < characterKilled.Count; i++)
			{
				textKilled.text += (" " + (characterKilled[i].PlayerID+1) + "P");
			}
			textKilled.text += " )</size>";
		}

		public void DrawKiller(List<CharacterBase> killer)
		{
			textKiller.text = killer.Count.ToString();
			textKiller.text += "<size=24> (";
			for (int i = 0; i < killer.Count; i++)
			{
				textKiller.text += (" " + (killer[i].PlayerID+1) + "P");
			}
			textKiller.text += " )</size>";
		}

		public void DrawPreferedMove(List<string> attackNames, List<int> attackNb)
		{
			if (attackNames.Count == 0)
				return;
			int bestNumber = 0;
			int bestIndex = 0;
			for (int i = 0; i < attackNb.Count; i++)
			{
				if(attackNb[i] > bestNumber)
				{
					bestNumber = attackNb[i];
					bestIndex = i;
				}
			}
			textBestAttack.text = attackNames[bestIndex].Substring(0, attackNames[bestIndex].Length - 7);

		}


		public void SetFeedback(string text)
		{
			feedback.text = text;
			animator.SetTrigger("Feedback");
		}
		public void SetReverseFeedback()
		{
			animator.SetTrigger("ReverseFeedback");
		}

	}
}
