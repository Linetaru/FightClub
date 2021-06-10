using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Menu
{
	public class MenuOptionAudio : MenuList, IControllable
	{
        [SerializeField]
        MenuButtonListController listHorizontal; // La liste est vide, c'est juste pour détecter si on appuies sur droite / gauche pour changer les sliders

        [SerializeField]
        SaveSoundVolume volumeSettings;

        [SerializeField]
        Slider[] sliders;

        int characterID = 0;


		public override void InitializeMenu()
		{
			base.InitializeMenu();

            sliders[0].handleRect.gameObject.GetComponent<Image>().color = Color.cyan;

            sliders[0].value = volumeSettings.MasterVolume;
            listEntry.DrawItemList(0, volumeSettings.MasterVolume.ToString());

            sliders[1].value = volumeSettings.MusicVolume;
            listEntry.DrawItemList(1, volumeSettings.MusicVolume.ToString());

            sliders[2].value = volumeSettings.VoiceVolume;
            listEntry.DrawItemList(2, volumeSettings.VoiceVolume.ToString());

            sliders[3].value = volumeSettings.SfxVolume;
            listEntry.DrawItemList(3, volumeSettings.SfxVolume.ToString());

            for (int i = 0; i < 4; i++)
            {
                if (sliders[i].value >= 50)
                {
                    foreach (TextMeshProUGUI tmpText in listEntry.ListItem[i].GetComponentsInChildren<TextMeshProUGUI>())
                        if (tmpText.name == "TextValue")
                            tmpText.color = Color.black;
                }
                else
                {
                    foreach (TextMeshProUGUI tmpText in listEntry.ListItem[i].GetComponentsInChildren<TextMeshProUGUI>())
                        if (tmpText.name == "TextValue")
                            tmpText.color = Color.white;
                }
            }
        }

		public override void UpdateControl(int id, Input_Info input)
		{
            if (characterID == id)
            {
                if (listEntry.InputList(input) == true) // On s'est déplacé dans la liste
                {
                    SelectEntry(listEntry.IndexSelection);
                }
                else if (listHorizontal.InputListHorizontal(input) == true) // On s'est déplacé dans la liste
                {
                    ChangeVolume(listEntry.IndexSelection, (int)Mathf.Sign(input.horizontal));
                }
                else if (input.inputUiAction == InputConst.Return)
                {
                    QuitMenu();
                }
            }
            else if (Mathf.Abs(input.vertical) > 0.2f)
            {
                characterID = id;
            }
        }


        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);

            for (int i = 0; i < 4; i++)
            {
                sliders[i].handleRect.gameObject.GetComponent<Image>().color = Color.white;
            }

            sliders[id].handleRect.gameObject.GetComponent<Image>().color = Color.cyan;
        }


		private void ChangeVolume(int id, int direction)
        {
            if (id == 0) // Master
            {
                volumeSettings.MasterVolume += 1 * direction;
                sliders[id].value = volumeSettings.MasterVolume;
                listEntry.DrawItemList(id, volumeSettings.MasterVolume.ToString());
            }
            else if (id == 1) // Music
            {
                volumeSettings.MusicVolume += 1 * direction;
                sliders[id].value = volumeSettings.MusicVolume;
                listEntry.DrawItemList(id, volumeSettings.MusicVolume.ToString());
                
            }
            else if(id == 2) // Voice
            {
                volumeSettings.VoiceVolume += 1 * direction;
                sliders[id].value = volumeSettings.VoiceVolume;
                listEntry.DrawItemList(id, volumeSettings.VoiceVolume.ToString());
            }
            else if(id == 3) // SFX
            {
                volumeSettings.SfxVolume += 1 * direction;
                sliders[id].value = volumeSettings.SfxVolume;
                listEntry.DrawItemList(id, volumeSettings.SfxVolume.ToString());
            }

            if (sliders[id].value >= 50)
            {
                foreach (TextMeshProUGUI tmpText in listEntry.ListItem[id].GetComponentsInChildren<TextMeshProUGUI>())
                    if (tmpText.name == "TextValue")
                        tmpText.color = Color.black;
            }
            else
            {
                foreach (TextMeshProUGUI tmpText in listEntry.ListItem[id].GetComponentsInChildren<TextMeshProUGUI>())
                    if (tmpText.name == "TextValue")
                        tmpText.color = Color.white;
            }

            volumeSettings.ChangeRTPCValue();
        }
	}
}
