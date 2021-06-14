using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Sirenix.OdinInspector;

// Editor only
using System.Linq;

[CreateAssetMenu(fileName = "InputProfile_", menuName = "Data/InputProfileData", order = 1)]
public class InputMappingData : ScriptableObject
{
    [SerializeField]
    public string profileName;

    [SerializeField]
    public EnumInput inputJump;
    [SerializeField]
    public EnumInput inputShortHop;
    [SerializeField]
    public EnumInput inputAttack;
    [SerializeField]
    public EnumInput inputSpecial;

    [SerializeField]
    public EnumInput inputParry;
    [SerializeField]
    public EnumInput inputDash;

    /*
     * Get the Player for which you are changing element mappings.
    Get the ControllerMap you want to modify from the Player.
    Various functions here can help in finding it. For example:
    player.controllers.maps.GetMap
    Listen for controller input using Input Mapper.
    Handle assignment conflicts.
    */


    /*[ActionIdProperty(typeof(int))]
    public int actionId;*/

   /* [ValueDropdown("GetMapAction")]//, IsUniqueList = true)]
    [SerializeField]
    public List<int> actionElementMapId;// { get; set; }*/


    public void Button()
    {
       // ReInput.userDataStore.SaveInputBehavior
       // ReInput.userDataStore.LoadControllerData
       /* ReInput.mapping.Actions;
        ReInput.players.Players[0].
        ReInput.mapping.Actions.
            Re*/
           
    }

    /*private IEnumerable GetMapAction()
    {
        Rewired.ControllerMap.
        if (Rewired.InputManager.)
        return ReInput.mapping.Actions.Select(x => new ValueDropdownItem(x.name, x.id));
    }*/
}
