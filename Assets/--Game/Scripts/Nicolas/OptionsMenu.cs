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

    [Title("Selection Input")]
    public GameObject panelSelection;
    public List<Image> selectionUI_Input_Arrow;
    public List<TMP_Text> text_Input;
    public MenuButtonListController listEntrySelection;


    public List<InputMappingDataClassic> inputConfig = new List<InputMappingDataClassic>();
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

        if (listEntry.IndexSelection < 6 && state == SelectionState.OnProfile)
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
                break;
            case SelectionState.OnSelectionInput:
                if (id == 6)
                {
                    panelSelection.SetActive(false);
                    listEntry = listEntryProfile;
                    listEntry.SelectIndex(0);
                }
                else if(id == 7)
                {
                    panelSelection.SetActive(false);
                    listEntry = listEntryProfile;
                    listEntry.SelectIndex(0);
                }
                break;
        }

    }

    public override void InitializeMenu()
    {
        base.InitializeMenu();

        inputConfig = InputMappingDataStatic.inputMappingDataClassics;

        foreach (Image im in selectionUI_Input_Arrow)
            im.gameObject.SetActive(false);

        inputVar = new EnumInput[]
        {
            profileSelected.inputMappingData.inputJump,
            profileSelected.inputMappingData.inputShortHop,
            profileSelected.inputMappingData.inputAttack,
            profileSelected.inputMappingData.inputSpecial,
            profileSelected.inputMappingData.inputParry,
            profileSelected.inputMappingData.inputDash,
        };

        Debug.Log("Est-ce que c'est appelé tout simplement ?");
        for (int i = 0; i < inputVar.Length; i++)
        {
            listEntry.DrawItemList(i, inputVar[i].ToString());
        }
    }

    protected override void SelectEntry(int id)
    {
        base.SelectEntry(id);
        foreach (Image im in selectionUI_Input_Arrow)
            im.gameObject.SetActive(false);
        selectionUI_Input_Arrow[id].gameObject.SetActive(true);

    }
}
