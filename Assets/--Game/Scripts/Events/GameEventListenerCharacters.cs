using UnityEngine.Events;
using UnityEngine;

namespace PackageCreator.Event
{
    [System.Serializable]
    public class UnityEventCharacters : UnityEvent<CharacterBase, CharacterBase> { }

    [AddComponentMenu("_PackageCreator/Game Event Listener/Characters Event", order: 49)]
    public class GameEventListenerCharacters : MonoBehaviour
    {
        [SerializeField]
        private GameEventCharacters gameEventCharacter;
        [SerializeField]
        private UnityEventCharacters response;

        private void OnEnable()
        {
            gameEventCharacter.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEventCharacter.UnregisterListener(this);
        }

        public void OnEventRaised(CharacterBase cb_Value, CharacterBase cb_Value2)
        {
            response.Invoke(cb_Value, cb_Value2);
        }
    }
}
