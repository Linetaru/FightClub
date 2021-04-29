﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIcon : MonoBehaviour
{
    [SerializeField]
    private GameObject iconPos;

    private Icon icon;

    public void CreateIcon(Icon iconGO)
    {
        icon = Instantiate(iconGO);
        icon.transform.parent = transform.root;
        icon.transform.position = iconPos.transform.position;
    }

    public void SwitchIcon()
    {
        icon.SwitchIcon();
    }

}
