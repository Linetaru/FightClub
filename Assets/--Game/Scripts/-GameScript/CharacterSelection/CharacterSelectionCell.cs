using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionCell : MonoBehaviour
{
    public CharacterData characterData;

    public enum Character
    {
        Bernard,
        Robotio,
        NinjaMurai,
        Katarina,
        Random,
        None
    }

    public Character character;
}
