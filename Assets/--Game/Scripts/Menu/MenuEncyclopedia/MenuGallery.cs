using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Menu
{
	public class MenuGallery : MenuList, IControllable
	{
		[SerializeField]
		InputController inputController;

		[SerializeField]
		SODatabase_Encyclopedia galleryDatabase;

		[SerializeField]
		MenuArtwork menuArtwork;

		[Title("UI")]
		[SerializeField]
		Image image;

		List<int> idItems = new List<int>();
		bool firstTime = false;


		private void Awake()
		{
			menuArtwork.OnQuit += BackToMenu;
		}

		private void OnDestroy()
		{
			menuArtwork.OnQuit -= BackToMenu;
		}



		public override void InitializeMenu()
		{
			base.InitializeMenu();
			if (!firstTime)
			{
				DrawEncyclopedia(galleryDatabase);
				firstTime = true;
			}
		}


		public void DrawEncyclopedia(SODatabase_Encyclopedia encyclopedia)
		{
			idItems.Clear();
			for (int i = 0; i < encyclopedia.Database.Count; i++)
			{
				Debug.Log(encyclopedia.Database[i]);
				if (encyclopedia.GetUnlocked(i))
				{
					listEntry.DrawItemList(i, null, encyclopedia.Database[i].EntryTitle);
					idItems.Add(i);
				}
			}
			if (idItems.Count == 0)
				return;
			SelectEntry(0);
			listEntry.SelectIndex(0);
			listEntry.SetItemCount(idItems.Count);
		}


		public override void UpdateControl(int id, Input_Info input)
		{
			if (listEntry.InputList(input) == true) // On s'est déplacé dans la liste
			{
				SelectEntry(listEntry.IndexSelection);
			}
			else if (input.inputUiAction == InputConst.Interact)
			{
				input.inputUiAction = null;
				ValidateEntry(listEntry.IndexSelection);
			}
			else if (input.inputUiAction == InputConst.Return)
			{
				QuitMenu();
			}
		}


		protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);
			image.sprite = galleryDatabase.Database[idItems[id]].EntrySprite;
		}


		protected override void ValidateEntry(int id)
		{
			base.SelectEntry(id);
			menuArtwork.InitializeMenu(galleryDatabase.Database[idItems[id]].EntrySprite, galleryDatabase.Database[idItems[id]].EntryText);
			inputController.controllable[0] = menuArtwork;

		}


		private void BackToMenu()
		{
			inputController.controllable[0] = this;
		}
	}
}
