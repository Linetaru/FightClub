using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PackageCreator.Event
{
    [CreateAssetMenu(fileName = "Game Event bool", menuName = "PackageCreator/Game Event/Game Event bool", order = 53)]
    public class GameEventBool : ScriptableObject
    {
        /// <summary>
        /// reference all listener in this list for this Event 
        /// </summary>
        private readonly List<GameEventListenerBool> eventListeners =
            new List<GameEventListenerBool>();

        public void Raise(bool tOrf)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(tOrf);
        }

        public void RegisterListener(GameEventListenerBool listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListenerBool listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
    }
}
