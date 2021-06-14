using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterSoundManager : MonoBehaviour
{

    [Space]
    [SerializeField]
    AK.Wwise.Event parrySuccessEvent = null;
    [SerializeField]
    AK.Wwise.Event parryFailedEvent = null;
    [SerializeField]
    AK.Wwise.Event parryBreakEvent = null;
    [Space]
    [SerializeField]
    AK.Wwise.Event clashEvent = null;
    [Space]
    [SerializeField]
    AK.Wwise.Event deathEvent = null;


    [Title("Voice Announcer")]
    [SerializeField]
    AK.Wwise.Event voiceAnnoucerBreakthrough = null;
    [SerializeField]
    AK.Wwise.Event voiceAnnoucerParry = null;




    List<CharacterBase> characters = new List<CharacterBase>();

    // Appelé par un event
    public void SetAudioCharacter(CharacterBase character)
    {
        character.OnStateChanged += CheckState;
        character.Knockback.Parry.OnClash += PlayClash;
        character.Knockback.Parry.OnGuard += PlayGuard;
        character.Knockback.Parry.OnParry += PlayParry;
        character.Knockback.Parry.OnGuardBreak += PlayGuardBreak;
        characters.Add(character);
    }



    public void CheckState(CharacterState oldState, CharacterState newState)
    {
        if (newState is CharacterStateDeath)
        {
            if (parryFailedEvent != null)
                AkSoundEngine.PostEvent(deathEvent.Id, this.gameObject);
        }
    }



    public void PlayParry(CharacterBase characterBase)
    {
        AkSoundEngine.PostEvent(parrySuccessEvent.Id, this.gameObject);
    }
    public void PlayGuard(CharacterBase characterBase, CharacterBase opponent)
    {
        //AkSoundEngine.PostEvent(parryFailedEvent.Id, this.gameObject);
    }
    public void PlayClash(AttackSubManager subManager)
    {
        AkSoundEngine.PostEvent(clashEvent.Id, this.gameObject);
        AkSoundEngine.PostEvent(voiceAnnoucerParry.Id, this.gameObject);
    }
    public void PlayGuardBreak(AttackSubManager subManager)
    {
        AkSoundEngine.PostEvent(parryBreakEvent.Id, this.gameObject);
        AkSoundEngine.PostEvent(voiceAnnoucerBreakthrough.Id, this.gameObject);
    }



    void OnDestroy()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].OnStateChanged -= CheckState;
            characters[i].Knockback.Parry.OnClash -= PlayClash;
            characters[i].Knockback.Parry.OnGuard -= PlayGuard;
            characters[i].Knockback.Parry.OnParry -= PlayParry;
            characters[i].Knockback.Parry.OnGuardBreak -= PlayGuardBreak;
        }
    }
}
