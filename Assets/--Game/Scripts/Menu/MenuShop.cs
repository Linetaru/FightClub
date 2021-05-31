using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;

namespace Menu
{
	public class MenuShop : MenuList, IControllable
	{
		[Space]
		[SerializeField]
		CurrencyData currencyData = null;


		[Title("Database")]
		[InfoBox("Mettre que des IUnlockable (des databases normalement)")]
		[SerializeField]
		List<ScriptableObject> database = new List<ScriptableObject>();
		List<IUnlockable> databaseUnlock = new List<IUnlockable>();

		[SerializeField]
		SODatabase_Shop databaseShop = null;

		[Title("UI")]
		[SerializeField]
		string menuTitle;
		[SerializeField]
		TextMeshProUGUI textMenuTitle;

		[SerializeField]
		Textbox textDescription;

		[Title("Animators")]
		[SerializeField]
		Animator animatorMenu;


		private void Start()
		{
			for (int i = 0; i < database.Count; i++)
			{
				IUnlockable interfac = database[i] as IUnlockable;
				if (interfac != null)
					databaseUnlock.Add(interfac);
			}
		}

		public override void InitializeMenu()
		{
			animatorMenu.gameObject.SetActive(true);
			animatorMenu.SetBool("Appear", true);

			for (int i = 0; i < databaseShop.Database.Count; i++)
			{
				if (GetUnlocked(databaseShop.Database[i].GetUnlockID()))
					listEntry.DrawItemList(i, null, databaseShop.Database[i].ItemName, "SOLD OUT");
				else
					listEntry.DrawItemList(i, null, databaseShop.Database[i].ItemName, databaseShop.Database[i].ItemPrice + " BP");
			}

			if (listEntry.ListItem.Count != 0)
			{
				SelectEntry(0);
				listEntry.SelectIndex(0);
				listEntry.SetItemCount(databaseShop.Database.Count);
			}


			textMenuTitle.text = menuTitle;
			base.InitializeMenu();
		}



		protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);

			//animatorDescription.SetTrigger("Feedback");
			textDescription.DrawTextbox(databaseShop.Database[id].ItemDescription);
		}

		protected override void ValidateEntry(int id)
		{
			base.ValidateEntry(id);

			if (!GetUnlocked(databaseShop.Database[id].GetUnlockID()))
			{

				if (currencyData.Money >= databaseShop.Database[id].ItemPrice)
				{
					currencyData.AddMoney(-databaseShop.Database[id].ItemPrice);
					listEntry.DrawItemList(id, null, databaseShop.Database[id].ItemName, "SOLD OUT");
				}
				SetUnlocked(databaseShop.Database[id].GetUnlockID());
			}

		}

		protected override void QuitMenu()
		{
			base.QuitMenu();
			animatorMenu.SetBool("Appear", false);
		}



		public void SetUnlocked(string itemName)
		{
			// Faire gaffe faut que les databaseUnlock ne contienne pas de database avec une référence partagé
			for (int i = 0; i < databaseUnlock.Count; i++)
			{
				databaseUnlock[i].SetUnlocked(itemName, true);
			}
		}
		public bool GetUnlocked(string itemName)
		{
			for (int i = 0; i < databaseUnlock.Count; i++)
			{
				if (databaseUnlock[i].GetUnlocked(itemName))
					return true;
			}
			return false;
		}


	}
}
