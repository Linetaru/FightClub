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


    [SerializeField]
    [SuffixLabel("en frames")]
    float bonusHitstun = 0;

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




    [Space]
    [Title("Feedback")]
    public float percentageSpawnParticle;
    [HorizontalGroup("2")]
    [HideLabel]
    public GameObject particleDirection;
    [HorizontalGroup("2")]
    public float timeBeforeDestroying;



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

            knockbackDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (knockbackAngle + angle)), -Mathf.Sin(Mathf.Deg2Rad * (knockbackAngle + angle)));
        }
        else
        {
            knockbackDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * knockbackAngle), Mathf.Sin(Mathf.Deg2Rad * knockbackAngle));
            knockbackDirection *= new Vector2(user.Movement.Direction, 1);
        }

        float knockbackValue = CalculateKnockback(target.Stats.LifePercentage) * 0.5f;
        target.Knockback.Launch(knockbackDirection, knockbackValue, bonusHitstun / 60f);

        float hitStopAmount = hitStop;
        user.SetMotionSpeed(0, hitStopAmount);
        target.SetMotionSpeed(0, hitStopAmount);


        if (target.Stats.LifePercentage >= percentageSpawnParticle && particleDirection != null)
        {
            GameObject go = Instantiate(particleDirection, target.CenterPoint.position, Quaternion.Euler(0, 0, -Mathf.Atan2(knockbackDirection.x, knockbackDirection.y) * Mathf.Rad2Deg));
            go.name = particleDirection.name;
            Destroy(go, timeBeforeDestroying);
        }

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
