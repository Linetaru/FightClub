using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PackageCreator.Event
{
    [CreateAssetMenu(fileName = "Game Event Int", menuName = "PackageCreator/Game Event/Game Event Int", order = 52)]
    public class GameEventInt : ScriptableObject
    {
        /// <summary>
        /// reference all listener in this list for this Event 
        /// </summary>
        private readonly List<GameEventListenerInt> eventListeners = new List<GameEventListenerInt>();

        public void Raise(int value)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(value);
        }

        public void RegisterListener(GameEventListenerInt listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListenerInt listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
    }
}