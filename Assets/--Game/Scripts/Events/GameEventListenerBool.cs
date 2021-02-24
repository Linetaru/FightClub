using UnityEngine.Events;
using UnityEngine;

namespace PackageCreator.Event
{
    [System.Serializable]
    public class UnityEventBool : UnityEvent<bool> { }

    [AddComponentMenu("_PackageCreator/Game Event Listener/Bool Event", order: 49)]
    public class GameEventListenerBool : MonoBehaviour
    {
        [SerializeField]
        private GameEventBool gameEventBool;
        [SerializeField]
        private UnityEventBool response;

        private void OnEnable()
        {
            gameEventBool.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEventBool.UnregisterListener(this);
        }

        public void OnEventRaised(bool tOrf)
        {
            response.Invoke(tOrf);
        }
    }
}
