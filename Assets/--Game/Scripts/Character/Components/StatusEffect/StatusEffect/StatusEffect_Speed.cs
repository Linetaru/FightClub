using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Speed : StatusEffect
{
    [SerializeField]
    private float speedMultiplier;

    public StatusEffect_Speed()
    {
    }

    public StatusEffect_Speed(float speed)
    {
        speedMultiplier = speed;
    }


    public override StatusEffect Copy()
    {
        return new StatusEffect_Speed(speedMultiplier);
    }

    public override void ApplyEffect(CharacterBase character)
    {
        character.Stats.Speed.IncrementMultiplierBonusStat(speedMultiplier);
        character.Movement.SpeedMax = character.Stats.Speed.Value;

    }

    public override void UpdateEffect(CharacterBase character)
    {

    }

    public override void RemoveEffect(CharacterBase character)
    {
        character.Stats.Speed.IncrementMultiplierBonusStat(-speedMultiplier);
        character.Movement.SpeedMax = character.Stats.Speed.Value;
    }
}