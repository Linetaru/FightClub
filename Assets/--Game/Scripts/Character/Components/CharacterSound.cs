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

    [Space]
    [SerializeField]
    AK.Wwise.Event parrySuccessEvent;
    [SerializeField]
    AK.Wwise.Event parryFailedEvent;


    [Space]
    [SerializeField]
    AK.Wwise.Event deathEvent;

    /*[Title("Voice")]
    [SerializeField]
    AK.Wwise.Event damageVoiceEvent;*/

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
            if (jumpEvent != null)
                AkSoundEngine.PostEvent(jumpEvent.Id, this.gameObject);
        }
        if (newState is CharacterStateLanding)
        {
            if (landEvent != null)
                AkSoundEngine.PostEvent(landEvent.Id, this.gameObject);
        }

        if (newState is CharacterStateParrySuccess)
        {
            if(parrySuccessEvent != null)
                AkSoundEngine.PostEvent(parrySuccessEvent.Id, this.gameObject);
        }
        if (newState is CharacterStateParryBlow)
        {
            if (parryFailedEvent != null)
                AkSoundEngine.PostEvent(parryFailedEvent.Id, this.gameObject);
        }
        if (newState is CharacterStateDeath)
        {
            if (parryFailedEvent != null)
                AkSoundEngine.PostEvent(deathEvent.Id, this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (characterBase == null)
            return;
        characterBase.OnStateChanged -= CheckState;
    }
}
