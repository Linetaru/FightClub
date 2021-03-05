using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Menu
{
	public class MenuButtonList : MonoBehaviour
	{
		[SerializeField]
		Image imageButton;
		[SerializeField]
		TextMeshProUGUI mainText;
		[SerializeField]
		TextMeshProUGUI subText;

		[SerializeField]
		Animator animator;

		private RectTransform rectTransform;
		public RectTransform RectTransform
		{
			get { if (rectTransform == null) rectTransform = GetComponent<RectTransform>(); return rectTransform; } // A corriger quand j'aurai moins la flemme
		}


		public virtual void DrawButton(Sprite icon, string text, string text2)
		{
			imageButton.sprite = icon;
			mainText.text = text;
			subText.text = text2;
		}

		public virtual void SelectButton()
		{
			animator.SetTrigger("Selected");
		}
		public virtual void UnselectButton()
		{
			animator.SetTrigger("Unselected");
		}
	}
}
