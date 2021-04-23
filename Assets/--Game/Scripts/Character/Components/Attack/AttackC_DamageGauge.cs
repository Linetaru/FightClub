using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_DamageGauge : AttackComponent
{

    [HorizontalGroup("Damage - Power")]
    [SerializeField]
    [LabelText("Damage Power")]
    float minDamage = 10;

    [HorizontalGroup("Damage - Power", Width = 0.4f)]
    [SerializeField]
    [HideLabel]
    AnimationCurve damageCurve;

    [HorizontalGroup("Damage - Power", Width = 40f)]
    [SerializeField]
    [HideLabel]
    float maxDamage = 50;

    [HorizontalGroup("Damage - Percentage")]
    [SerializeField]
    float minPowerGauge = 0;
    [HorizontalGroup("Damage - Percentage")]
    [SerializeField]
    float maxPowerGauge = 100;


    float damage = 0;
    public override void StartComponent(CharacterBase user)
    {
        // on fait ça dans le start puisqu'on perd aussi de la jauge dans le start
        // Du coup au moment du hit, la jauge est toujours vide donc le calcul est faussé
        damage = minDamage + ((maxDamage - minDamage) * damageCurve.Evaluate((user.PowerGauge.CurrentPower - minPowerGauge) / (maxPowerGauge - minPowerGauge)));
    }

    public override void UpdateComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        target.Stats.TakeDamage((int)damage);
    }

    public override void EndComponent(CharacterBase user)
    {

    }
}
