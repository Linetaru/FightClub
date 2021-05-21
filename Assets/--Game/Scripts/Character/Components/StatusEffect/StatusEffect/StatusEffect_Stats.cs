using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StatusEffect_Stats : StatusEffect
{
    [SerializeField]
    [HideReferenceObjectPicker]
    [HideLabel]
    StatModifier statModifier;


    public StatusEffect_Stats()
    {

    }

    public StatusEffect_Stats(StatModifier statModifier)
    {
        this.statModifier = statModifier;
    }


    public override StatusEffect Copy()
    {
        return new StatusEffect_Stats(statModifier);
    }

    public override void ApplyEffect(CharacterBase character)
    {
        statModifier.ApplyModifier(character.Stats);
    }

    public override void UpdateEffect(CharacterBase character)
    {

    }

    public override void RemoveEffect(CharacterBase character)
    {
        statModifier.RevertModifier(character.Stats);
    }
}

