using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum KnockbackAngleSetting
{
    StaticAngle,
    DynamicAngle
};


public class AttackC_Knockback : AttackComponent
{
    [Title("HitStop")]
    [SerializeField]
    float hitStop = 0.1f;



    [Title("Ejection - Power")]
    [SerializeField]
    bool knockbackAdvancedSettings = true;


    [ShowIf("knockbackAdvancedSettings")]
    [HorizontalGroup("Knockback - Power")]
    [SerializeField]
    [LabelText("Knockback Power")]
    float minKnockbackPower = 10;

    [ShowIf("knockbackAdvancedSettings")]
    [HorizontalGroup("Knockback - Power", Width = 0.4f)]
    [SerializeField]
    [HideLabel]
    AnimationCurve knockbackCurve;

    [ShowIf("knockbackAdvancedSettings")]
    [HorizontalGroup("Knockback - Power", Width = 40f)]
    [SerializeField]
    [HideLabel]
    float maxKnockbackPower = 50;





    [ShowIf("knockbackAdvancedSettings")]
    [HorizontalGroup("Knockback - Percentage")]
    [SerializeField]
    float minCurvePercentage = 0;
    [ShowIf("knockbackAdvancedSettings")]
    [HorizontalGroup("Knockback - Percentage")]
    [SerializeField]
    float maxCurvePercentage = 100;




    [SerializeField]
    float linearKnockbackPower = 0.5f;


    [FoldoutGroup("Ejection Power Calculator")]
    [OnValueChanged("DebugCalculation")]
    [SerializeField]
    float percentage = 0;
    [FoldoutGroup("Ejection Power Calculator")]
    [ReadOnly]
    [SerializeField]
    float ejectionPowerTheoric = 0;



    [Space]
    [Title("Ejection - Angle")]
    [SerializeField] 
    float knockbackAngle = 0;

    // L'angle dynamique signifie que l'angle de trajectoire se fait par rapport aux positions des personnages
    [SerializeField]
    KnockbackAngleSetting dynamicAngle = KnockbackAngleSetting.StaticAngle;





    public override void StartComponent(CharacterBase user)
    {

    }

    public override void UpdateComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        Vector2 knockbackDirection;
        if (dynamicAngle == KnockbackAngleSetting.DynamicAngle)
        {
            Vector2 targetDirection = (target.transform.position - user.transform.position).normalized;
            float angle = Vector2.SignedAngle(targetDirection, Vector2.right);

            knockbackDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (knockbackAngle + angle)), Mathf.Sin(Mathf.Deg2Rad * (knockbackAngle + angle)));
        }
        else
        {
            knockbackDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * knockbackAngle), Mathf.Sin(Mathf.Deg2Rad * knockbackAngle));
            knockbackDirection *= new Vector2(user.Movement.Direction, 1);
        }

        target.Knockback.Launch(knockbackDirection, CalculateKnockback(target.Stats.LifePercentage) * 0.5f);




        user.SetMotionSpeed(0, hitStop);
        target.SetMotionSpeed(0, hitStop);
    }

    public override void EndComponent(CharacterBase user)
    {

    }



    private void DebugCalculation()
    {
        ejectionPowerTheoric = CalculateKnockback(percentage);
    }

    private float CalculateKnockback(float percentage)
    {
        if (knockbackAdvancedSettings == false)
            return (linearKnockbackPower * percentage);

        if (percentage < minCurvePercentage)
            return minKnockbackPower;
        else if (percentage > maxCurvePercentage)
            return maxKnockbackPower + ((linearKnockbackPower * maxKnockbackPower) * ((percentage / maxCurvePercentage) - 1));

        float factor = knockbackCurve.Evaluate((percentage - minCurvePercentage) / (maxCurvePercentage - minCurvePercentage));
        return minKnockbackPower + ((maxKnockbackPower - minKnockbackPower) * factor);
    }


}
