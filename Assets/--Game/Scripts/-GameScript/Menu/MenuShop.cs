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
		[Title("Database")]
		[SerializeField]
		CurrencyData currencyData = null;
		[SerializeField]
		SODatabase_Shop databaseShop = null;

		[Title("UI")]
		[SerializeField]
		string menuTitle = "";
		[SerializeField]
		TextMeshProUGUI textMenuTitle = null;

		[SerializeField]
		Textbox textDescription = null;

		[Title("Animators")]
		[SerializeField]
		Animator animatorMenu = null;
		[SerializeField]
		Animator animatorUnityChan = null;

		/*private void Start()
		{
			for (int i = 0; i < database.Count; i++)
			{
				IUnlockable interfac = database[i] as IUnlockable;
				if (interfac != null)
					databaseUnlock.Add(interfac);
			}
		}*/

		public override void InitializeMenu()
		{
			animatorMenu.gameObject.SetActive(true);
			animatorMenu.SetBool("Appear", true);

			for (int i = 0; i < databaseShop.Database.Count; i++)
			{
				if (GetUnlocked(databaseShop.Database[i].GetUnlockID(), databaseShop.Database[i].DatabaseToLook))
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

			animatorUnityChan.SetTrigger("Feedback");
			textDescription.DrawTextbox(databaseShop.Database[id].ItemDescription);
		}

		protected override void ValidateEntry(int id)
		{
			//base.ValidateEntry(id);

			if (!GetUnlocked(databaseShop.Database[id].GetUnlockID(), databaseShop.Database[id].DatabaseToLook))
			{
				if (currencyData.Money >= databaseShop.Database[id].ItemPrice)
				{
					base.ValidateEntry(id);
					currencyData.AddMoney(-databaseShop.Database[id].ItemPrice);
					listEntry.DrawItemList(id, null, databaseShop.Database[id].ItemName, "SOLD OUT");
					SetUnlocked(databaseShop.Database[id].GetUnlockID(), databaseShop.Database[id].DatabaseToLook);
					animatorUnityChan.SetTrigger("Buy");
				}

			}

		}

		protected override void QuitMenu()
		{
			base.QuitMenu();
			animatorMenu.SetBool("Appear", false);
		}





		public void SetUnlocked(string itemName, ScriptableObject so)
		{
			IUnlockable unlockable = so as IUnlockable;
			if (so != null)
				SetUnlocked(itemName, unlockable);
			else
				Debug.LogWarning("The database to look in order to buy this item is wrong");
		}

		public void SetUnlocked(string itemName, IUnlockable databaseToLook)
		{
			databaseToLook.SetUnlocked(itemName, true);
		}


		public bool GetUnlocked(string itemName, ScriptableObject so)
		{
			IUnlockable unlockable = so as IUnlockable;
			if(so != null)
				return GetUnlocked(itemName, unlockable);
			else
				Debug.LogWarning("The database to look in order to buy this item is wrong");
			return false;
		}

		public bool GetUnlocked(string itemName, IUnlockable databaseToLook)
		{
			return databaseToLook.GetUnlocked(itemName);
		}


	}
}
