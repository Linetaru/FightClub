using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AK;

[RequireComponent(typeof(AkGameObj))]
public class CharacterSound : MonoBehaviour
{

	[SerializeField]
	CharacterBase characterBase;


    [Title("Sounds")]
    [SerializeField]
    AK.Wwise.Event jumpEvent;
    [SerializeField]
    AK.Wwise.Event landEvent;

    [Title("Voice")]
    [SerializeField]
    AK.Wwise.Event damageVoiceEvent;

    void Start()
    {
        if (characterBase == null)
            return;
        characterBase.OnStateChanged += CheckState;
    }

    public void CheckState(CharacterState oldState, CharacterState newState)
    {
        if(newState is CharacterStateStartJump)
        {
            AkSoundEngine.PostEvent(jumpEvent.Id, this.gameObject);
        }
        if (newState is CharacterStateLanding)
        {
            AkSoundEngine.PostEvent(landEvent.Id, this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (characterBase == null)
            return;
        characterBase.OnStateChanged -= CheckState;
    }
}
