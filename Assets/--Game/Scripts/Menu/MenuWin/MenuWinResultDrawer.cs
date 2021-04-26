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

		public void SetFeedback(string text)
		{
			animator.SetTrigger("Feedback");
		}
		public void SetReverseFeedback()
		{
			animator.SetTrigger("ReverseFeedback");
		}
	}
}
