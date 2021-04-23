using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Stats : StatusEffect
{
    [SerializeField]
    bool attack = true;
    [SerializeField]
    float addStat = 1;
    [SerializeField]
    Color colorFlash = Color.green;

    float t = 0f;

    public StatusEffect_Stats()
    {
    }

    public StatusEffect_Stats(bool isAttack, float stat, Color c)
    {
        attack = isAttack;
        addStat = stat;
        colorFlash = c;
    }


    public override StatusEffect Copy()
    {
        return new StatusEffect_Stats(attack, addStat, colorFlash);
    }

    public override void ApplyEffect(CharacterBase character)
    {
        if(attack == true)
            character.Stats.AttackMultiplier.IncrementFlatBonusStat(addStat);
        else
            character.Stats.DefenseMultiplier.IncrementFlatBonusStat(addStat);
    }

    public override void UpdateEffect(CharacterBase character)
    {
        t -= Time.deltaTime * character.MotionSpeed;
        if(t <= 0)
        {
            character.Model.FlashModel(colorFlash, 0.2f);
            t = 0.3f;
        }

    }

    public override void RemoveEffect(CharacterBase character)
    {
        if (attack == true)
            character.Stats.AttackMultiplier.IncrementFlatBonusStat(-addStat);
        else
            character.Stats.DefenseMultiplier.IncrementFlatBonusStat(-addStat);
    }
}
