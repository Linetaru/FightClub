using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Stats : StatusEffect
{
    [SerializeField]
    float addStat = 1;
    float t = 0f;

    public StatusEffect_Stats()
    {
    }

    public StatusEffect_Stats(float stat)
    {
        addStat = stat;
    }


    public override StatusEffect Copy()
    {
        return new StatusEffect_Stats(addStat);
    }

    public override void ApplyEffect(CharacterBase character)
    {
        character.Stats.AttackMultiplier.IncrementFlatBonusStat(addStat);
    }

    public override void UpdateEffect(CharacterBase character)
    {
        t -= Time.deltaTime * character.MotionSpeed;
        if(t <= 0)
        {
            character.Model.FlashModel(Color.green, 0.2f);
            t = 0.3f;
        }

    }

    public override void RemoveEffect(CharacterBase character)
    {
        character.Stats.AttackMultiplier.IncrementFlatBonusStat(-addStat);
    }
}
