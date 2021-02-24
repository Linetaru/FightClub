using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace PackageCreator.Event
{
    [AddComponentMenu("_PackageCreator/Game Event Listener/Void Event", order: 50)]
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField]
        private GameEvent gameEvent;
        [SerializeField]
        private UnityEvent response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}
