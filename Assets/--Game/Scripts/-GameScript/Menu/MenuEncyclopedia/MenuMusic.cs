using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AK.Wwise;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{
	public class MenuMusic : MenuList, IControllable
	{

		[SerializeField]
		SODatabase_Encyclopedia databaseMusic = null;

		[Title("Logic")]
		[SerializeField]
		float fadeTime = 0.5f;
		[SerializeField]
		Color colorButtonActive = Color.white;
		[SerializeField]
		Color colorButtonInactive = Color.white;

		[Title("UI")]
		[SerializeField]
		TextMeshProUGUI textDescription = null;
		[SerializeField]
		Image imagePlay = null;
		[SerializeField]
		Image imagePause = null;

		bool firstTime = false;
		bool musicPlaying = false;
		bool musicPause = false;

		List<int> idItems = new List<int>();
		AK.Wwise.Event currentSound = null;

		private void OnDestroy()
		{
			if (currentSound != null)
				currentSound.ExecuteAction(this.gameObject, AkActionOnEventType.AkActionOnEventType_Stop, (int)(fadeTime * 1000), AkCurveInterpolation.AkCurveInterpolation_Linear);
		}

		public override void InitializeMenu()
		{
			base.InitializeMenu();
			if (!firstTime)
			{
				DrawEncyclopedia(databaseMusic);
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


		/*public override void UpdateControl(int id, Input_Info input)
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
		}*/


		protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);
			textDescription.text = databaseMusic.Database[idItems[id]].EntryText;

			musicPause = false;
			musicPlaying = false;

			imagePlay.color = colorButtonActive;
			imagePause.color = colorButtonInactive;

			if (currentSound != null)
				currentSound.ExecuteAction(this.gameObject, AkActionOnEventType.AkActionOnEventType_Stop, (int)(fadeTime * 1000), AkCurveInterpolation.AkCurveInterpolation_Linear);
		}


		protected override void ValidateEntry(int id)
		{
			base.SelectEntry(id);

			if(musicPlaying == false)
			{
				musicPlaying = true;
				currentSound = databaseMusic.Database[idItems[id]].EventSound;
				AkSoundEngine.PostEvent(currentSound.Id, this.gameObject);

				imagePlay.color = colorButtonInactive;
				imagePause.color = colorButtonActive;
			}
			else if (musicPlaying == true && musicPause == false)
			{
				musicPause = true;
				currentSound.ExecuteAction(this.gameObject, AkActionOnEventType.AkActionOnEventType_Pause, 1, AkCurveInterpolation.AkCurveInterpolation_Linear);

				imagePlay.color = colorButtonActive;
				imagePause.color = colorButtonInactive;
			}
			else if (musicPlaying == true && musicPause == true)
			{
				musicPause = false;
				currentSound.ExecuteAction(this.gameObject, AkActionOnEventType.AkActionOnEventType_Resume, 1, AkCurveInterpolation.AkCurveInterpolation_Linear);

				imagePlay.color = colorButtonInactive;
				imagePause.color = colorButtonActive;
			}

		}
	}
}
