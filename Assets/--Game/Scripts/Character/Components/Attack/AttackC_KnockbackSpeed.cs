using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_KnockbackSpeed : AttackComponent
{
    [Title("HitStop")]
    [SerializeField]
    float hitStop = 0.1f;


    [SerializeField]
    [SuffixLabel("en frames")]
    float bonusHitstun = 0;

    [Title("Ejection - Power")]
    [SerializeField]
    float knockbackSpeedMultiplier = 1f;




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



    // Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
		
    }
	
	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
		
    }
	
	// Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        // Calcul de l'angle (simillaire a AttackC_Knockback donc y'a un truc a optimiser)
        Vector2 knockbackDirection;
        if (dynamicAngle == KnockbackAngleSetting.DynamicAngle)
        {
            Vector2 targetDirection = (target.transform.position - user.transform.position).normalized;
            float angle = Vector2.SignedAngle(targetDirection, Vector2.right);

            knockbackDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (knockbackAngle + angle)), -Mathf.Sin(Mathf.Deg2Rad * (knockbackAngle + angle)));
        }
        else if (dynamicAngle == KnockbackAngleSetting.StaticAngle)
        {
            knockbackDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * knockbackAngle), Mathf.Sin(Mathf.Deg2Rad * knockbackAngle));
            knockbackDirection *= new Vector2(user.Movement.Direction, 1);
        }
        else
        {
            knockbackDirection = new Vector2(user.Movement.SpeedX * user.Movement.Direction, user.Movement.SpeedY);
            knockbackDirection.Normalize();
        }


        float knockbackValue = new Vector2(user.Movement.SpeedX, user.Movement.SpeedY).magnitude * knockbackSpeedMultiplier;
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

    public override void OnParry(CharacterBase user, CharacterBase target)
    {

    }
    public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
    {

    }
    public override void OnClash(CharacterBase user, CharacterBase target)
    {

    }

    // Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
    {
		
    }
}
