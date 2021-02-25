using UnityEngine.Events;
using UnityEngine;

namespace PackageCreator.Event
{
    [System.Serializable]
    public class UnityEventFloat : UnityEvent<float> { }

    [AddComponentMenu("_PackageCreator/Game Event Listener/Float Event", order: 49)]
    public class GameEventListenerFloat : MonoBehaviour
    {
        [SerializeField]
        private GameEventFloat gameEventFloat;
        [SerializeField]
        private UnityEventFloat response;

        private void OnEnable()
        {
            gameEventFloat.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEventFloat.UnregisterListener(this);
        }

        public void OnEventRaised(float f_Value)
        {
            response.Invoke(f_Value);
        }
    }
}
