using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Menu
{
	// J'en peux plus sauvez moi
	[System.Serializable]
	public class MenuMainButtonTable
	{
		[HorizontalGroup]
		[HideLabel]
		public MenuMainButton buttons1;
		[HorizontalGroup]
		[HideLabel]
		public MenuMainButton buttons2;
		[HorizontalGroup]
		[HideLabel]
		public MenuMainButton buttons3;

		public MenuMainButton GetButton(int id)
		{
			if (id == 0)
				return buttons1;
			else if (id == 1)
				return buttons2;
			else if (id == 2)
				return buttons3;
			else
				return null;
		}
	}

	public class MenuMainGameModes : MenuList, IControllable
	{
		[SerializeField]
		MenuButtonListController listHorizontal;

		[Space]
		[SerializeField]
		Vector2Int defaultIndex;
		[SerializeField]
		MenuMainButtonTable[] menuMainButtons;

		[SerializeField]
		UnityEvent eventQuit;

		int characterID = 0;
		int x; int y;

		public override void InitializeMenu()
		{
			base.InitializeMenu();
			x = defaultIndex.x;
			y = defaultIndex.y;
		}


		public override void UpdateControl(int id, Input_Info input)
		{
			if(characterID == id)
			{
				if (listEntry.InputList(input) == true)
				{
					menuMainButtons[y].GetButton(x).Unselected();

					// C'est dla merde, dla belle merde en barquette
					for (int i = 0; i < menuMainButtons.Length; i++)
					{
						y -= 1 * (int)Mathf.Sign(input.vertical);
						if (y < 0)
							y = menuMainButtons.Length-1;
						if (y >= menuMainButtons.Length)
							y = 0;
						if (menuMainButtons[y].GetButton(x) != null)
							break;

						for (int j = 0; j < 3; j++)
						{
							x += 1;
							if (x < 0)
								x = 2;
							if (x >= 3)
								x = 0;
							if (menuMainButtons[y].GetButton(x) != null)
							{
								i = menuMainButtons.Length;
								break;
							}
						}
					}
					// ==============================================

					menuMainButtons[y].GetButton(x).Selected();
				}
				else if (listHorizontal.InputListHorizontal(input) == true)
				{
					menuMainButtons[y].GetButton(x).Unselected();

					for (int i = 0; i < 3; i++)
					{
						x += 1 * (int)Mathf.Sign(input.horizontal);
						if (x < 0)
							x = 2;
						if (x >= 3)
							x = 0;
						if (menuMainButtons[y].GetButton(x) != null)
							break;
					}
					menuMainButtons[y].GetButton(x).Selected();
				}
				else if (input.inputUiAction == InputConst.Interact)
				{
					input.inputUiAction = null;
					ValidateEntry(listEntry.IndexSelection);
				}
				else if (input.inputUiAction == InputConst.Return)
				{
					input.inputUiAction = null;
					eventQuit.Invoke();
				}
			}
			else if (Mathf.Abs(input.horizontal) > 0.2f || Mathf.Abs(input.vertical) > 0.2f)
			{
				characterID = id;
			}
		}



		/*protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);

			menuMainButtons[y].GetButton(x).Selected();
		}*/

		protected override void ValidateEntry(int id)
		{
			base.SelectEntry(id);
			menuMainButtons[y].GetButton(x).CallEvent();
		}
	}
}
