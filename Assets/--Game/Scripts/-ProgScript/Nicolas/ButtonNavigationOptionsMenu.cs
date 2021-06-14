using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menu;
using Sirenix.OdinInspector;

public class ButtonNavigationOptionsMenu : MenuList
{
    public List<Image> selectionUIArrow = new List<Image>();

    [Title("MenuList")]
    public List<MenuList> inputOptionsMenu = new List<MenuList>();

    [Title("Panel Reference")]
    public GameObject panelInput;
    public GameObject panelAudio;
    public GameObject panelGraph;

    [Title("Object Reference")]
    public MenuMainGameModes menuMainGameModes;
    public InputController inputController;
    public Canvas canvasOption;

    private int characterID = 0;

    public override void UpdateControl(int id, Input_Info input)
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
        else if (input.inputUiAction == InputConst.Interact)
        {
            input.inputUiAction = null;
            ValidateEntry(listEntry.IndexSelection);
        }
        else if (input.inputUiAction == InputConst.Return)
        {
            input.inputUiAction = null;
            SaveManager.Instance.SaveFile();
            QuitMenu();
        }
    }

    public override void InitializeMenu()
    {
        base.InitializeMenu();
        for(int i = 0; i < inputOptionsMenu.Count; i++)
        {
            inputOptionsMenu[i].OnEnd += OnEndMenuList;
        }

        this.OnEnd += OnEndThisMenu;

        OnEndMenuList();
    }

    private void OnEndThisMenu()
    {
        for (int i = 0; i < 4; i++)
            inputController.controllable[i] = menuMainGameModes;

        canvasOption.gameObject.SetActive(false);
    }

    private void OnEndMenuList()
    {
        panelInput.SetActive(false);
        panelAudio.SetActive(false);
        panelGraph.SetActive(false);
        selectionUIArrow[listEntry.IndexSelection].gameObject.SetActive(true);
        MovingArrow(listEntry.IndexSelection);
        for (int i = 0; i < 4; i++)
            inputController.controllable[i] = this;
    }

    private void MovingArrow(int index)
    {
        //selectionUIArrow[listEntry.IndexSelection].rectTransform.anchoredPosition = listEntry.ListItem[index].RectTransform.anchoredPosition;
        //selectionUIArrow[listEntry.IndexSelection].rectTransform.anchoredPosition += new Vector2(-6, 0);
        foreach(Image im in selectionUIArrow)
        {
            if(selectionUIArrow[listEntry.IndexSelection].gameObject == im.gameObject)
            {
                im.gameObject.SetActive(true);
            }
            else
                im.gameObject.SetActive(false);
        }
    }

    protected override void ValidateEntry(int id)
    {
        base.ValidateEntry(id);
        if(id == 0)
        {
            DisplayPanel(panelInput, id);
        }
        else if(id == 1)
        {
            DisplayPanel(panelAudio, id);
        }
        else if(id == 2)
        {
            DisplayPanel(panelGraph, id);
        }
        inputOptionsMenu[id].InitializeMenu();
    }

    public void DisplayPanel(GameObject panel, int index)
    {
        panel.SetActive(true);
        selectionUIArrow[listEntry.IndexSelection].gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
            inputController.controllable[i] = inputOptionsMenu[index];
    }

    protected override void SelectEntry(int id)
    {
        base.SelectEntry(id);
        MovingArrow(id);
    }
}
