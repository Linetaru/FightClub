using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PackageCreator.Event
{
    [CreateAssetMenu(fileName = "Game Event characters", menuName = "PackageCreator/Game Event/Game Event characters", order = 53)]
    public class GameEventCharacters : ScriptableObject
    {
        /// <summary>
        /// reference all listener in this list for this Event 
        /// </summary>
        private readonly List<GameEventListenerCharacters> eventListeners =
            new List<GameEventListenerCharacters>();

        public void Raise(CharacterBase cb_Value, CharacterBase cb_Value2)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(cb_Value, cb_Value2);
        }

        public void RegisterListener(GameEventListenerCharacters listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListenerCharacters listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
    }
}
