using UnityEngine.Events;
using UnityEngine;

namespace PackageCreator.Event
{
    [System.Serializable]
    public class UnityEventUICharacter : UnityEvent</*CharacterBase, float,*/ int> { }

    [AddComponentMenu("_PackageCreator/Game Event Listener/Character Event", order: 49)]
    public class GameEventListenerUICharacter : MonoBehaviour
    {
        [SerializeField]
        private GameEventUICharacter gameEventCharacter;
        [SerializeField]
        private UnityEventUICharacter response;

        private void OnEnable()
        {
            gameEventCharacter.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEventCharacter.UnregisterListener(this);
        }

        public void OnEventRaised(/*CharacterBase cb_Value, float f_value,*/ int i_value)
        {
            response.Invoke(/*cb_Value, f_value,*/ i_value);
        }

    }
}
