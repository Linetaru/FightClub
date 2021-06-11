using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PackageCreator.Event
{
    //[CreateAssetMenu(fileName = "Game Event T", menuName = "PackageCreator/Game Event/Game Event T", order = 52)]
    //public class GameEventT : ScriptableObject
    //{
    //    /// <summary>
    //    /// reference all listener in this list for this Event 
    //    /// </summary>
    //    private readonly List<GameEventListenerT> eventListeners = new List<GameEventListenerT>();

    //    public void Raise()
    //    {
    //        for (int i = eventListeners.Count - 1; i >= 0; i--)
    //            eventListeners[i].OnEventRaised();
    //    }

    //    public void RegisterListener(GameEventListenerT listener)
    //    {
    //        if (!eventListeners.Contains(listener))
    //        {
    //            eventListeners.Add(listener);
    //        }
    //    }

    //    public void UnregisterListener(GameEventListenerT listener)
    //    {
    //        if (eventListeners.Contains(listener))
    //        {
    //            eventListeners.Remove(listener);
    //        }
    //    }
    //}
}