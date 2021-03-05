using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerSelectionCoin : MonoBehaviour
{
    enum PlayerNumber
    {
        One,
        Two,
        Three,
        Four
    }

    [SerializeField]
    PlayerNumber playerNumber;

    [SerializeField]
    PlayerSelectionFrame playerFrame;

    [SerializeField]
    float cursorSpeed = 250f;

    bool isCharacterSelected = false;

    public bool isAboveCell = false;

    //CharacterData characterData;

    GraphicRaycaster graphicRaycaster;
    PointerEventData pointerEventData = new PointerEventData(null);

    Player player;

    [SerializeField]
    LayerMask CharacterCellLayer;

    RectTransform rectTransform;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        switch (playerNumber)
        {
            case PlayerNumber.One:
                player = ReInput.players.GetPlayer(0);
                break;

            case PlayerNumber.Two:
                player = ReInput.players.GetPlayer(1);
                break;

            case PlayerNumber.Three:
                player = ReInput.players.GetPlayer(2);
                break;

            case PlayerNumber.Four:
                player = ReInput.players.GetPlayer(3);
                break;

            default:
                player = null;
                break;
        }
    }

    private void Update()
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + player.GetAxis("Horizontal") * cursorSpeed * Time.deltaTime,
            rectTransform.anchoredPosition.y + player.GetAxis("Vertical") * cursorSpeed * Time.deltaTime);

        pointerEventData.position = Camera.main.ScreenToWorldPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        if(results.Count > 0)
        {
            Debug.Log(results[0].gameObject.name);
        }

        //if (Physics.Raycast(rectTransform.anchoredPosition, Vector3.forward, out _, 5f, CharacterCellLayer) && !isAboveCell)
        //{
        //    playerFrame.SetActiveCharacterPortrait(true);
        //    Debug.Log("ActivePortrait");
        //    isAboveCell = true;
        //}
        //else
        //{
        //    playerFrame.SetActiveCharacterPortrait(false);
        //    isAboveCell = false;
        //}
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<CharacterSelectionCell>() != null)
    //    {
    //        playerFrame.SetActiveCharacterPortrait(true);
    //        Debug.Log("ActivePortrait");
    //        isAboveCell = true;
    //    }
    //    else
    //    {
    //        playerFrame.SetActiveCharacterPortrait(false);
    //        isAboveCell = false;
    //    }
    //}
}
