using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionFrame : MonoBehaviour
{
    [SerializeField]
    GameObject pressButtonText;

    [SerializeField]
    GameObject playerBox;

    [SerializeField]
    Color playerColor;

    Color CPUColor = Color.gray;

    [SerializeField]
    Image frameOutline;

    [SerializeField]
    Image characterPortrait;

    [SerializeField]
    TextMeshProUGUI characterName;

    public void DisplayFrame(bool isCPU)
    {
        pressButtonText.SetActive(false);
        playerBox.SetActive(true);

        ChangeOutlineColor(isCPU);
    }

    public void ChangeOutlineColor(bool isCPU)
    {
        if (isCPU)
            frameOutline.color = CPUColor;
        else
            frameOutline.color = playerColor;
    }

    public void HideFrame()
    {
        pressButtonText.SetActive(true);
        playerBox.SetActive(false);
    }

    public void SetActiveCharacterPortrait(bool active)
    {
        if (active != characterPortrait.gameObject.activeSelf)
            characterPortrait.gameObject.SetActive(active);
    }

    public void ChangeCharacterPortrait(Sprite newCharacterSprite)
    {
        characterPortrait.sprite = newCharacterSprite;
    }
}
