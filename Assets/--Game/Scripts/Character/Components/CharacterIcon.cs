using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIcon : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer iconRenderer;

    public void SwitchIcon()
    {
        if (iconRenderer.enabled)
            iconRenderer.enabled = false;
        else
            iconRenderer.enabled = true;
    }

}
