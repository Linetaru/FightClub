using UnityEngine.Events;
using UnityEngine;

namespace PackageCreator.Event
{
    [System.Serializable]
    public class UnityEventCharacter : UnityEvent<CharacterBase> { }

    [AddComponentMenu("_PackageCreator/Game Event Listener/Character Event", order: 49)]
    public class GameEventListenerCharacter : MonoBehaviour
    {
        [SerializeField]
        private GameEventCharacter gameEventCharacter;
        [SerializeField]
        private UnityEventCharacter response;

        private void OnEnable()
        {
            gameEventCharacter.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEventCharacter.UnregisterListener(this);
        }

        public void OnEventRaised(CharacterBase cb_Value)
        {
            response.Invoke(cb_Value);
        }
    }
}
