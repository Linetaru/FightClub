using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AK;

[RequireComponent(typeof(AkGameObj))]
public class CharacterSound : MonoBehaviour
{

    [SerializeField]
    CharacterBase characterBase = null;


    [Title("Sounds")]
    [SerializeField]
    AK.Wwise.Event jumpEvent = null;
    [SerializeField]
    AK.Wwise.Event doubleJumpEvent = null;
    [SerializeField]
    AK.Wwise.Event landEvent = null;


    [Title("Voice")]
    [SerializeField]
    AK.Wwise.Event damageVoiceEvent = null;
    [SerializeField]
    AK.Wwise.Event ejectionVoiceEvent = null;
    [SerializeField]
    AK.Wwise.Event parryVoiceEvent = null;
    [SerializeField]
    AK.Wwise.Event guardVoiceEvent = null;
    [SerializeField]
    AK.Wwise.Event breakVoiceEvent = null;

    void Start()
    {
        if (characterBase == null)
            return;
        characterBase.OnStateChanged += CheckState;
        characterBase.Movement.OnMultipleJump += PlayMultipleJump;
        characterBase.Knockback.OnKnockback += PlayKnockback;
        characterBase.Knockback.Parry.OnParry += PlayParry;
        characterBase.Knockback.Parry.OnGuard += PlayGuard;
        characterBase.Knockback.Parry.OnGuardBreak += PlayGuardBreak;
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
        if (newState is CharacterStateDeath)
        {
            if (ejectionVoiceEvent != null)
                AkSoundEngine.PostEvent(ejectionVoiceEvent.Id, this.gameObject);
        }
        if (newState is CharacterStateKnockback)
        {
            AkSoundEngine.StopAll(characterBase.Model.gameObject);
        }
    }


    public void PlayMultipleJump()
    {
        AkSoundEngine.PostEvent(doubleJumpEvent.Id, this.gameObject);
    }
    public void PlayKnockback(AttackSubManager subManager)
    {
        AkSoundEngine.PostEvent(damageVoiceEvent.Id, this.gameObject);
    }

    public void PlayParry(CharacterBase characterBase)
    {
        AkSoundEngine.PostEvent(parryVoiceEvent.Id, this.gameObject);
    }

    public void PlayGuard(CharacterBase characterBase, CharacterBase opponent)
    {
        AkSoundEngine.PostEvent(guardVoiceEvent.Id, this.gameObject);
    }

    public void PlayGuardBreak(AttackSubManager subManager)
    {
        AkSoundEngine.PostEvent(breakVoiceEvent.Id, this.gameObject);
    }



    void OnDestroy()
    {
        if (characterBase == null)
            return;
        characterBase.OnStateChanged -= CheckState;
        characterBase.Movement.OnMultipleJump -= PlayMultipleJump;
        characterBase.Knockback.OnKnockback -= PlayKnockback;
        characterBase.Knockback.Parry.OnParry -= PlayParry;
        characterBase.Knockback.Parry.OnGuard -= PlayGuard;
        characterBase.Knockback.Parry.OnGuardBreak -= PlayGuardBreak;
    }
}
