using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PackageCreator.Event
{
    [CreateAssetMenu(fileName = "Game Event float", menuName = "PackageCreator/Game Event/Game Event float", order = 53)]
    public class GameEventFloat : ScriptableObject
    {
        /// <summary>
        /// reference all listener in this list for this Event 
        /// </summary>
        private readonly List<GameEventListenerFloat> eventListeners =
            new List<GameEventListenerFloat>();

        public void Raise(float f_Value)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(f_Value);
        }

        public void RegisterListener(GameEventListenerFloat listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListenerFloat listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
    }
}
