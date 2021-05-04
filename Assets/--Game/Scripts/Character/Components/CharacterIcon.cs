using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIcon : MonoBehaviour
{
    [SerializeField]
    private GameObject iconPos;

    public Icon icon;
    public Icon Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    public void CreateIcon(Icon iconGO)
    {
        icon = Instantiate(iconGO);
        icon.transform.parent = transform.root;
        icon.transform.position = iconPos.transform.position;
    }

    public void DestroyIcon()
    {
        Destroy(icon.gameObject);
    }

    public void SwitchIcon(bool param)
    {
        icon.SwitchIcon(param);
    }

}
