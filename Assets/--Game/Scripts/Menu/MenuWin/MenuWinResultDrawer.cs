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
			textControllerID.text = controllerID + "P";
			this.gameObject.SetActive(true);
		}

		public void SetFeedback(string text)
		{
			animator.SetTrigger("Feedback");
		}
	}
}
