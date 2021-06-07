using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using Menu;

public class OptionsMenu : MenuList
{
    [TitleGroup("Selection Profile")]
    public List<Image> selectionUI_Profile_Arrow;
    public List<TMP_Text> text_Profile;
    public MenuButtonListController listEntryProfile;
    public Image arrowBase;
    public GameObject profileBase;
    public ButtonInputData profileSelected;
    public GameObject panelArrowProfile;

    [Title("Selection Input")]
    public GameObject panelSelection;
    public GameObject panelArrowSelection;
    public List<Image> selectionUI_Input_Arrow;
    public List<TMP_Text> text_Input;
    public MenuButtonListController listEntrySelection;


    private List<InputMappingDataClassic> inputConfig = new List<InputMappingDataClassic>();
    EnumInput[] inputVar = new EnumInput[6];

    enum SelectionState{
        OnButton,
        OnProfile,
        OnSelectionInput,
    }

    SelectionState state = SelectionState.OnProfile;

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

        if (listEntry.IndexSelection < 6 && state == SelectionState.OnSelectionInput)
        {
            if (input.CheckActionAbsolute(0, InputConst.Attack))
                inputVar[listEntry.IndexSelection] = EnumInput.A;
            else if(input.CheckActionAbsolute(0, InputConst.Smash))
                inputVar[listEntry.IndexSelection] = EnumInput.B;
            else if (input.CheckActionAbsolute(0, InputConst.Special))
                inputVar[listEntry.IndexSelection] = EnumInput.X;
            else if (input.CheckActionAbsolute(0, InputConst.Jump))
                inputVar[listEntry.IndexSelection] = EnumInput.Y;
            else if (input.CheckActionAbsolute(0, InputConst.RightShoulder))
                inputVar[listEntry.IndexSelection] = EnumInput.R1;
            else if (input.CheckActionAbsolute(0, InputConst.RightTrigger))
                inputVar[listEntry.IndexSelection] = EnumInput.R2;

            if (input.inputActions.Count > 0)
            {
                listEntry.DrawItemList(listEntry.IndexSelection, inputVar[listEntry.IndexSelection].ToString());
                listEntry.SelectIndex(listEntry.IndexSelection + 1);
                SelectEntry(listEntry.IndexSelection);
                input.inputActions.Clear();
            }
        }
    }

    protected override void ValidateEntry(int id)
    {
        base.ValidateEntry(id);

        switch (state)
        {
            case SelectionState.OnButton:
                break;
            case SelectionState.OnProfile:
                if(id > 0)
                {
                    panelSelection.SetActive(true);
                    panelArrowSelection.SetActive(true);
                    profileSelected = listEntry.ListItem[listEntry.IndexSelection].GetComponent<ButtonInputData>();
                    listEntry = listEntrySelection;
                    listEntry.SelectIndex(0);
                    selectionUI_Input_Arrow[0].gameObject.SetActive(true);
                    state = SelectionState.OnSelectionInput;
                    Init();
                }
                break;
            case SelectionState.OnSelectionInput:
                if (id == 6)
                {
                    panelSelection.SetActive(false);
                    panelArrowSelection.SetActive(false);
                    listEntry = listEntryProfile;
                    listEntry.SelectIndex(0);
                    selectionUI_Profile_Arrow[0].gameObject.SetActive(true);
                    state = SelectionState.OnProfile;
                }
                else if(id == 7)
                {
                    state = SelectionState.OnProfile;
                    panelSelection.SetActive(false);
                    panelArrowSelection.SetActive(false);
                    int indexTMP = listEntryProfile.ListItem.IndexOf(profileSelected.gameObject.GetComponent<MenuButtonList>());
                    Destroy(selectionUI_Profile_Arrow[indexTMP].gameObject);
                    selectionUI_Profile_Arrow.RemoveAt(indexTMP);
                    listEntryProfile.ListItem.Remove(profileSelected.gameObject.GetComponent<MenuButtonList>());
                    for (int i = 0; i < InputMappingDataStatic.inputMappingDataClassics.Count; i++)
                    {
                        if (InputMappingDataStatic.inputMappingDataClassics[i].profileName == profileSelected.inputMappingData.profileName)
                            InputMappingDataStatic.inputMappingDataClassics.RemoveAt(i);
                    }
                    Destroy(profileSelected.gameObject);
                    profileSelected = null;
                    listEntry = listEntryProfile;
                    listEntry.UpdateCountList();
                    listEntry.SelectIndexForIndexOnly(0);
                    selectionUI_Profile_Arrow[0].gameObject.SetActive(true);
                }
                break;
        }

    }

    public void Init()
    {
        InputMappingDataStatic.inputMappingDataClassics.Add(new InputMappingDataClassic("test"));
        InputMappingDataStatic.inputMappingDataClassics.Add(new InputMappingDataClassic("test2"));

        switch (state)
        {
            case SelectionState.OnButton:
                break;
            case SelectionState.OnProfile:
                inputConfig = InputMappingDataStatic.inputMappingDataClassics;

                for (int i = 1; i < inputConfig.Count; i++)
                {
                    listEntry.DrawItemList(i, inputConfig[i].profileName);
                    Image im = Instantiate(arrowBase, panelArrowProfile.transform);
                    selectionUI_Profile_Arrow.Add(im);
                    Vector3 tmpPos = im.rectTransform.position;
                    tmpPos.y = listEntry.ListItem[i].gameObject.transform.position.y;
                    im.rectTransform.position = tmpPos;
                }

                inputVar = new EnumInput[]
                {
                    profileSelected.inputMappingData.inputJump,
                    profileSelected.inputMappingData.inputShortHop,
                    profileSelected.inputMappingData.inputAttack,
                    profileSelected.inputMappingData.inputSpecial,
                    profileSelected.inputMappingData.inputParry,
                    profileSelected.inputMappingData.inputDash,
                };

                panelSelection.SetActive(false);
                panelArrowSelection.SetActive(false);

                foreach (Image im in selectionUI_Profile_Arrow)
                    im.gameObject.SetActive(false);

                break;
            case SelectionState.OnSelectionInput:

                foreach (Image im in selectionUI_Input_Arrow)
                    im.gameObject.SetActive(false);

                for (int i = 0; i < inputVar.Length; i++)
                {
                    listEntry.DrawItemList(i, inputVar[i].ToString());
                }
                break;
        }
    }

    public override void InitializeMenu()
    {
        base.InitializeMenu();

        Init();
    }

    protected override void SelectEntry(int id)
    {
        base.SelectEntry(id);
        switch (state)
        {
            case SelectionState.OnButton:
                break;
            case SelectionState.OnProfile:
                foreach (Image im in selectionUI_Profile_Arrow)
                    im.gameObject.SetActive(false);
                selectionUI_Profile_Arrow[id].gameObject.SetActive(true);
                break;
            case SelectionState.OnSelectionInput:
                foreach (Image im in selectionUI_Input_Arrow)
                    im.gameObject.SetActive(false);
                selectionUI_Input_Arrow[id].gameObject.SetActive(true);
                break;
        }

    }
}
