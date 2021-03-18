using UnityEngine.Events;
using UnityEngine;

namespace PackageCreator.Event
{
    [System.Serializable]
    public class UnityEventInt : UnityEvent<int> { }

    [AddComponentMenu("_PackageCreator/Game Event Listener/Int Event", order: 49)]
    public class GameEventListenerInt : MonoBehaviour
    {
        [SerializeField]
        private GameEventInt gameEventBool;
        [SerializeField]
        private UnityEventInt response;

        private void OnEnable()
        {
            gameEventBool.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEventBool.UnregisterListener(this);
        }

        public void OnEventRaised(int value)
        {
            response.Invoke(value);
        }
    }
}
