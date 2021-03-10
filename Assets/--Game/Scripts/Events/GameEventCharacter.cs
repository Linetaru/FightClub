using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PackageCreator.Event
{
    [CreateAssetMenu(fileName = "Game Event character", menuName = "PackageCreator/Game Event/Game Event character", order = 53)]
    public class GameEventCharacter : ScriptableObject
    {
        /// <summary>
        /// reference all listener in this list for this Event 
        /// </summary>
        private readonly List<GameEventListenerCharacter> eventListeners =
            new List<GameEventListenerCharacter>();

        public void Raise(CharacterBase cb_Value)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(cb_Value);
        }

        public void RegisterListener(GameEventListenerCharacter listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListenerCharacter listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
    }
}
