using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
	public class MenuOptionGraphics : MenuList, IControllable
	{
        [SerializeField]
        MenuButtonListController listHorizontal; // La liste est vide, c'est juste pour détecter si on appuies sur droite / gauche pour changer les sliders

        [SerializeField]
        SaveGraphicsSettings graphicsSettings;

        public List<Image> selectionUIArrow = new List<Image>();

        int characterID = 0;

        [SerializeField]
        AK.Wwise.Event eventVolume;

        public override void InitializeMenu()
		{
			base.InitializeMenu();
            //graphicsSettings.Resolution = Screen.resolutions.Length - 1;

            listEntry.DrawItemList(0, QualitySettings.names[graphicsSettings.QualityLevel]);
            listEntry.DrawItemList(1, Screen.resolutions[graphicsSettings.Resolution].ToString());
            listEntry.DrawItemList(2, (graphicsSettings.Fullscreen == 0) ? "On" : "Off");
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
                    AkSoundEngine.PostEvent(eventVolume.Id, this.gameObject);
                    ChangeSettings(listEntry.IndexSelection, (int)Mathf.Sign(input.horizontal));
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

            //selectionUIArrow.rectTransform.anchoredPosition = listEntry.ListItem[id].RectTransform.anchoredPosition;
            //selectionUIArrow.rectTransform.anchoredPosition += new Vector2(-20, 0);
            foreach (Image im in selectionUIArrow)
            {
                if (selectionUIArrow[listEntry.IndexSelection].gameObject == im.gameObject)
                {
                    im.gameObject.SetActive(true);
                }
                else
                    im.gameObject.SetActive(false);
            }
        }


		private void ChangeSettings(int id, int direction)
        {
            if (id == 0) // Graphics
            {
                graphicsSettings.QualityLevel += 1 * direction;
                listEntry.DrawItemList(id, QualitySettings.names[graphicsSettings.QualityLevel]);
            }
            else if (id == 1) // Resolutions
            {
                graphicsSettings.Resolution += 1 * direction;
                listEntry.DrawItemList(id, Screen.resolutions[graphicsSettings.Resolution].ToString());
            }
            else if (id == 2) // Fullscreen
            {
                graphicsSettings.Fullscreen += 1 * direction;
                if (graphicsSettings.Fullscreen == 0)
                {
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                }
                else
                {
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                }
                listEntry.DrawItemList(id, (graphicsSettings.Fullscreen == 0) ? "On" : "Off");
            }
            graphicsSettings.ChangeQualitySettings();
        }
	}
}
