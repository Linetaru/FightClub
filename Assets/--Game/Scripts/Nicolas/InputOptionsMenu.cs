using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using Menu;

public class InputOptionsMenu : MenuList
{
    [TitleGroup("Selection Profile")]
    public RectTransform selectionUIArrow;
    public List<Image> selectionUI_Profile_Arrow;
    //public List<TMP_Text> text_Profile;
    public MenuButtonListController listEntryProfile;
    public Image arrowBase;
    public GameObject profileBase;
    public ButtonInputData profileSelected;
    public GameObject panelArrowProfile;
    public TMP_InputField inputField;

    [Title("Selection Input")]
    public GameObject panelSelection;
    public GameObject panelArrowSelection;
    public List<Image> selectionUI_Input_Arrow;
    //public List<TMP_Text> text_Input;
    public MenuButtonListController listEntrySelection;

    [Title("Name Input")]
    public InputController inputController;
    public MenuNameInput menuNameInput;

    private int characterID = 0;
    private List<InputMappingDataClassic> inputConfig = new List<InputMappingDataClassic>();
    EnumInput[] inputVar = new EnumInput[6];

    enum SelectionState{
        OnButton,
        OnProfile,
        OnSelectionInput,
    }

    SelectionState state = SelectionState.OnProfile;

    private void Awake()
    {
        menuNameInput.OnValidate += OnCreateProfile;
    }
    private void OnDestroy()
    {
        menuNameInput.OnValidate -= OnCreateProfile;
    }

    public override void UpdateControl(int id, Input_Info input)
    {
        if (state == SelectionState.OnSelectionInput)
        {
            if (characterID != id)
                return;
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
                else if (input.CheckActionAbsolute(0, InputConst.Smash))
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



        else if (state == SelectionState.OnProfile)
        {
            if (id == characterID)
            {
                if (listEntry.InputList(input) == true) // On s'est déplacé dans la liste
                {
                    SelectEntry(listEntry.IndexSelection);
                    return;
                }
            }
            if (Mathf.Abs(input.vertical) > 0.2f)
            {
                characterID = id;
            }

            if (input.inputUiAction == InputConst.Interact)
            {
                input.inputUiAction = null;
                input.inputActions.Clear();
                characterID = id;
                ValidateEntry(listEntry.IndexSelection);
            }
            else if (input.inputUiAction == InputConst.Return)
            {
                QuitMenu();
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
                if (id > 0)
                {
                    panelSelection.SetActive(true);
                    panelArrowSelection.SetActive(true);
                    profileSelected = listEntry.ListItem[listEntry.IndexSelection].GetComponent<ButtonInputData>();
                    profileSelected.inputMappingData.profileName = profileSelected.GetComponent<TextMeshProUGUI>().text;
                    listEntry = listEntrySelection;
                    listEntry.SelectIndex(0);
                    selectionUI_Input_Arrow[0].gameObject.SetActive(true);
                    state = SelectionState.OnSelectionInput;
                    Init();
                }
                else if(id == 0)
                {
                    for (int i = 0; i < inputController.controllable.Length; i++)
                    {
                        inputController.controllable[i] = null;
                    }
                    inputController.controllable[characterID] = menuNameInput;
                    menuNameInput.gameObject.SetActive(true);
                }
                break;
            case SelectionState.OnSelectionInput:
                if (id == 6)
                {
                    state = SelectionState.OnProfile;
                    panelSelection.SetActive(false);
                    panelArrowSelection.SetActive(false);
                    listEntry = listEntryProfile;
                    //int indexTMP = listEntryProfile.ListItem.IndexOf(profileSelected.gameObject.GetComponent<MenuButtonList>());
                    listEntry.SelectIndex(listEntryProfile.IndexSelection);
                    //selectionUI_Profile_Arrow[indexTMP].gameObject.SetActive(true);

                    InputMappingDataStatic.inputMappingDataClassics[listEntryProfile.IndexSelection - 1].inputJump = inputVar[0];
                    InputMappingDataStatic.inputMappingDataClassics[listEntryProfile.IndexSelection - 1].inputShortHop = inputVar[1];
                    InputMappingDataStatic.inputMappingDataClassics[listEntryProfile.IndexSelection - 1].inputAttack = inputVar[2];
                    InputMappingDataStatic.inputMappingDataClassics[listEntryProfile.IndexSelection - 1].inputSpecial = inputVar[3];
                    InputMappingDataStatic.inputMappingDataClassics[listEntryProfile.IndexSelection - 1].inputParry = inputVar[4];
                    InputMappingDataStatic.inputMappingDataClassics[listEntryProfile.IndexSelection - 1].inputDash = inputVar[5];

                    profileSelected.inputMappingData = InputMappingDataStatic.inputMappingDataClassics[listEntryProfile.IndexSelection - 1];
                }
                else if(id == 7)
                {
                    state = SelectionState.OnProfile;
                    panelSelection.SetActive(false);
                    panelArrowSelection.SetActive(false);
                    listEntry = listEntryProfile;

                    InputMappingDataStatic.inputMappingDataClassics.RemoveAt(listEntryProfile.IndexSelection-1);

                    Init();
                    listEntry.SelectIndexForIndexOnly(0);
                    SelectEntry(0);

                    //selectionUI_Profile_Arrow[0].gameObject.SetActive(true);
                }
                break;
        }

    }

    public void OnCreateProfile(string profileNameString)
    {
        // Redonne le controle du menu
        for (int i = 0; i < inputController.controllable.Length; i++)
        {
            inputController.controllable[i] = this;
        }
        menuNameInput.gameObject.SetActive(false);

        inputConfig = InputMappingDataStatic.inputMappingDataClassics;

        for (int i = 0; i < inputConfig.Count; i++)
        {
            if(inputConfig[i].profileName == profileNameString)
            {
                return;
            }
            /*if(listEntry.ListItem[i] != null)
            {
                foreach(MenuButtonList mbl in listEntry.ListItem)
                {
                    if (mbl.gameObject.GetComponent<TMP_Text>().text == inputConfig[i-1].profileName)
                    {
                        break;
                    }
                    else if(listEntry.ListItem.IndexOf(mbl) >= listEntry.ListItem.Count)
                    {
                        AddingNewProfile(i);
                    }
                }
            }*/
        }
        InputMappingDataStatic.inputMappingDataClassics.Add(new InputMappingDataClassic(profileNameString));

        //AddingNewProfile(inputConfig.Count);
        listEntry.DrawItemList(inputConfig.Count, profileNameString);
        //Init();
    }

    private void AddingNewProfile(int index)
    {
        //listEntry.DrawItemList(index, inputConfig[index].profileName);
        /*Image im = Instantiate(arrowBase, panelArrowProfile.transform);
        selectionUI_Profile_Arrow.Add(im);*/

        //Need improvement Image don't go more down than the first image spawned
       /* Vector3 tmpPos = im.transform.position;
        tmpPos.y = listEntry.ListItem[index].transform.position.y;
        im.transform.position = tmpPos;*/
    }

    public void Init()
    {
        //Testing Adding Some Input config when create button doesn't work yet
        /*    InputMappingDataStatic.inputMappingDataClassics.Add(new InputMappingDataClassic("test"));
            InputMappingDataStatic.inputMappingDataClassics.Add(new InputMappingDataClassic("test2"));*/

        switch (state)
        {
            case SelectionState.OnButton:
                break;
            case SelectionState.OnProfile:

                inputConfig = InputMappingDataStatic.inputMappingDataClassics;

                for (int i = 0; i < inputConfig.Count; i++)
                {
                    Debug.Log(inputConfig[i].profileName);
                    listEntry.DrawItemList(i+1, inputConfig[i].profileName);
                    //AddingNewProfile(i+1);
                }
                listEntryProfile.SetItemCount(inputConfig.Count + 1);
                panelSelection.SetActive(false);
                panelArrowSelection.SetActive(false);

               /* foreach (Image im in selectionUI_Profile_Arrow)
                    im.gameObject.SetActive(false);

                selectionUI_Profile_Arrow[listEntry.IndexSelection].gameObject.SetActive(true);*/

                break;
            case SelectionState.OnSelectionInput:


                inputVar = new EnumInput[]
                {
                    profileSelected.inputMappingData.inputJump,
                    profileSelected.inputMappingData.inputShortHop,
                    profileSelected.inputMappingData.inputAttack,
                    profileSelected.inputMappingData.inputSpecial,
                    profileSelected.inputMappingData.inputParry,
                    profileSelected.inputMappingData.inputDash,
                };

                foreach (Image im in selectionUI_Input_Arrow)
                    im.gameObject.SetActive(false);

                for (int i = 0; i < inputVar.Length; i++)
                {
                    listEntry.DrawItemList(i, inputVar[i].ToString());
                }

                selectionUI_Input_Arrow[listEntry.IndexSelection].gameObject.SetActive(true);
                break;
        }
    }

    public override void InitializeMenu()
    {
        base.InitializeMenu();

        Init();
        selectionUIArrow.anchoredPosition = listEntry.ListItem[0].RectTransform.anchoredPosition;
    }

    protected override void SelectEntry(int id)
    {
        base.SelectEntry(id);
        switch (state)
        {
            case SelectionState.OnButton:
                break;
            case SelectionState.OnProfile:
                selectionUIArrow.anchoredPosition = listEntry.ListItem[id].RectTransform.anchoredPosition;
                /*foreach (Image im in selectionUI_Profile_Arrow)
                    im.gameObject.SetActive(false);
                selectionUI_Profile_Arrow[id].gameObject.SetActive(true);*/
                break;
            case SelectionState.OnSelectionInput:
                foreach (Image im in selectionUI_Input_Arrow)
                    im.gameObject.SetActive(false);
                selectionUI_Input_Arrow[id].gameObject.SetActive(true);
                break;
        }

    }
}
