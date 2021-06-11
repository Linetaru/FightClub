using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PackageCreator.Event
{
    [CreateAssetMenu(fileName = "Game Event keycode", menuName = "PackageCreator/Game Event/Game Event keycode", order = 52)]
    public class GameEventKeycode : ScriptableObject
    {
        /// <summary>
        /// reference all listener in this list for this Event 
        /// </summary>
        private readonly List<GameEventListenerKeycode> eventListeners = new List<GameEventListenerKeycode>();

        public void Raise(KeyCode keycode)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(keycode);
        }

        public void RegisterListener(GameEventListenerKeycode listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListenerKeycode listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
    }
}