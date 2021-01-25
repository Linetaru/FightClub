using UnityEngine.Events;
using UnityEngine;

namespace PackageCreator.Event
{
    [System.Serializable]
    public class UnityEventKeycode : UnityEvent<KeyCode> { }

    [AddComponentMenu("_PackageCreator/Game Event Listener/Bool Event", order: 49)]
    public class GameEventListenerKeycode : MonoBehaviour
    {
        [SerializeField]
        private GameEventKeycode gameEventKeycode;
        [SerializeField]
        private UnityEventKeycode response;

        private void OnEnable()
        {
            gameEventKeycode.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEventKeycode.UnregisterListener(this);
        }

        public void OnEventRaised(KeyCode keycode)
        {
            response.Invoke(keycode);
        }
    }
}