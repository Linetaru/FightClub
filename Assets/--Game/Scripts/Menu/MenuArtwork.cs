using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Menu
{
	public class MenuArtwork : MonoBehaviour, IControllable
	{
		[SerializeField]
		GameObject menu;
		[SerializeField]
		Image image;
		[SerializeField]
		TextMeshProUGUI textDescription;

		[SerializeField]
		GameObject panelDescription;


		public event EventVoid OnQuit;
		bool hideUI = true;


		public void InitializeMenu(Sprite sprite, string description)
		{
			menu.gameObject.SetActive(true);
			image.sprite = sprite;
			//image.SetNativeSize();
			textDescription.text = description;
			hideUI = true;
			panelDescription.gameObject.SetActive(!hideUI);
		}


		public void UpdateControl(int id, Input_Info input)
		{
			if (input.inputUiAction == InputConst.Interact)
			{
				input.inputUiAction = null;
				hideUI = !hideUI;
				panelDescription.gameObject.SetActive(!hideUI);
			}
			else if (input.inputUiAction == InputConst.Return)
			{
				input.inputUiAction = null;
				QuitMenu();
			}
		}

		public void QuitMenu()
		{
			menu.gameObject.SetActive(false);
			OnQuit?.Invoke();
		}
	}
}
