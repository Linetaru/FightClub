using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menu;
using Sirenix.OdinInspector;

public class ButtonNavigationOptionsMenu : MenuList
{
    public Image selectionUIArrow;

    [Title("MenuList")]
    public List<MenuList> inputOptionsMenu = new List<MenuList>();

    [Title("Panel Reference")]
    public GameObject panelInput;
    public GameObject panelAudio;
    public GameObject panelGraph;

    [Title("Object Reference")]
    public MenuManagerUpdated menuPrincipal;
    public InputController inputController;

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

        this.OnEnd += menuPrincipal.CloseOptions;

        OnEndMenuList();
    }

    private void OnEndMenuList()
    {
        panelInput.SetActive(false);
        panelAudio.SetActive(false);
        panelGraph.SetActive(false);
        selectionUIArrow.gameObject.SetActive(true);
        MovingArrow(listEntry.IndexSelection);
        for (int i = 0; i < 4; i++)
            inputController.controllable[i] = this;
    }

    private void MovingArrow(int index)
    {
        selectionUIArrow.rectTransform.anchoredPosition = listEntry.ListItem[index].RectTransform.anchoredPosition;
        selectionUIArrow.rectTransform.anchoredPosition += new Vector2(-6, 0);
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
    }

    public void DisplayPanel(GameObject panel, int index)
    {
        panel.SetActive(true);
        selectionUIArrow.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
            inputController.controllable[i] = inputOptionsMenu[index];
    }

    protected override void SelectEntry(int id)
    {
        base.SelectEntry(id);
        MovingArrow(id);
    }
}
