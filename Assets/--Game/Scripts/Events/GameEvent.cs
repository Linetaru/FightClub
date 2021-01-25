using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PackageCreator.Event
{
    [CreateAssetMenu(fileName = "Game Event void", menuName = "PackageCreator/Game Event/Game Event void", order = 52)]
    public class GameEvent : ScriptableObject
    {
        /// <summary>
        /// reference all listener in this list for this Event 
        /// </summary>
        private readonly List<GameEventListener> eventListeners =
            new List<GameEventListener>();

        public void Raise()
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
    }
}
