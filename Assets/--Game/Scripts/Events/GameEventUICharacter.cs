using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PackageCreator.Event
{
    [CreateAssetMenu(fileName = "Game Event UICharacter", menuName = "PackageCreator/Game Event/Game Event UICharacter", order = 52)]
    public class GameEventUICharacter : ScriptableObject
    {
        /// <summary>
        /// reference all listener in this list for this Event 
        /// </summary>
        private readonly List<GameEventListenerUICharacter> eventListeners = new List<GameEventListenerUICharacter>();

        public void Raise(/*CharacterBase cb_Value, float f_value,*/ int i_value)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(/*cb_Value, f_value,*/ i_value);
        }

        public void RegisterListener(GameEventListenerUICharacter listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListenerUICharacter listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
    }
}