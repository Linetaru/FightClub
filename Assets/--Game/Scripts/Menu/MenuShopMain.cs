using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Menu
{
	public class MenuShopMain : MenuList, IControllable
	{

		[Title("Menu")]
		[SerializeField]
		InputController inputController;
		[SerializeField]
		MenuShop[] menuShops;
		[SerializeField]
		string[] textOptions;
		[SerializeField]
		Textbox textDescription;


		[Title("Animators")]
		[SerializeField]
		Animator animatorMenu;
		[SerializeField]
		Animator animatorDescription;
		[SerializeField]
		Animator animatorPanelUnityChan;
		[SerializeField]
		Animator animatorBackground;

		[SerializeField]
		Animator animatorPanelUnityChan2;

		void Awake()
		{
			for (int i = 0; i < menuShops.Length; i++)
			{
				menuShops[i].OnEnd += BackToMenu;
			}
		}

		void OnDestroy()
		{
			for (int i = 0; i < menuShops.Length; i++)
			{
				menuShops[i].OnEnd -= BackToMenu;
			}
		}




		public override void InitializeMenu()
		{
			SelectEntry(0);
			listEntry.SelectIndex(0);
			base.InitializeMenu();
		}

		protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);
			textDescription.DrawTextbox(textOptions[id]);
			animatorPanelUnityChan2.SetTrigger("Feedback");
			//animatorDescription.SetTrigger("Feedback");
		}

		protected override void ValidateEntry(int id)
		{
			base.ValidateEntry(id);

			inputController.controllable[0] = menuShops[id];
			menuShops[id].InitializeMenu();
			HideMenu();
		}

		protected override void QuitMenu()
		{
			SaveManager.Instance.SaveFile();
			base.QuitMenu();
		}






		private void BackToMenu()
		{
			inputController.controllable[0] = this;
			ShowMenu();
		}





		private void ShowMenu()
		{
			animatorMenu.SetBool("Appear", true);
			animatorBackground.SetBool("Right", false);
			animatorPanelUnityChan.SetBool("Right", false);
			animatorDescription.SetBool("Right", false);
		}

		private void HideMenu()
		{
			animatorMenu.SetBool("Appear", false);
			animatorBackground.SetBool("Right", true);
			animatorPanelUnityChan.SetBool("Right", true);
			animatorDescription.SetBool("Right", true);
		}








	}
}
