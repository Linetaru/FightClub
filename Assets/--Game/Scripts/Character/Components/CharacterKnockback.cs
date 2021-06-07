using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Feedbacks;

public class CharacterKnockback : MonoBehaviour
{
    private Vector3 contactPoint;
    public Vector3 ContactPoint
    {
        get { return contactPoint; }
        set { contactPoint = value; }
    }

    [Title("States")]
    [SerializeField]
    CharacterState stateKnockback = null;


    [Title("Parry")]
    [SerializeField]
    private CharacterParry parry = null;
    public CharacterParry Parry
    {
        get { return parry; }
    }



    [Title("Parameter")]
    [SerializeField]
    private float weight = 1;
    public float Weight
    {
        get { return weight; }
        set { weight = value; }
    }

    [SerializeField]
    private float timeKnockbackPerDistance = 0.025f;
    public float TimeKnockbackPerDistance
    {
        get { return timeKnockbackPerDistance; }
        set { timeKnockbackPerDistance = value; }
    }

    [SerializeField]
    private float knockbackAcumulationModifier = 0.4f;
    public float KnockbackAcumulationModifier
    {
        get { return knockbackAcumulationModifier; }
        set { knockbackAcumulationModifier = value; }
    }

    [SerializeField]
    private float maxTimeKnockback = 1.5f;
    public float MaxTimeKnockback
    {
        get { return maxTimeKnockback; }
    }

    private float knockbackDuration = 0;
    public float KnockbackDuration
    {
        get { return knockbackDuration; }
        set { knockbackDuration = value; }
    }

    private bool isArmor = false;
    public bool IsArmor
    {
        get { return isArmor; }
        set { isArmor = value; }
    }

    private bool isInvulnerable = false;
    public bool IsInvulnerable
    {
        get { return isInvulnerable; }
        set { isInvulnerable = value; }
    }

    private bool isHardKnockback = false;
    public bool IsHardKnockback
    {
        get { return isHardKnockback; }
        set { isHardKnockback = value; }
    }





    protected float motionSpeed = 1;
    public float MotionSpeed
    {
        get { return motionSpeed; }
        set { motionSpeed = value; }
    }



    private Vector2 angleKnockback;

    public EventAttackSubManager OnKnockback;
    List<AttackSubManager> atkRegistered = new List<AttackSubManager>();

    [Title("FeedbackComponents")]
    [SerializeField]
    private ShakeEffect shakeEffect;
    public ShakeEffect ShakeEffect
    {
        get { return shakeEffect; }
    }



    public bool CanKnockback()
    {
        if (knockbackDuration <= 0)
            return false;
        return true;
    }


    /*public void Hit()
    {
        // Event onHit
    }*/


    public void RegisterHit(AttackSubManager attack)
    {
        if (!atkRegistered.Contains(attack))
            atkRegistered.Add(attack);
    }

    public void UnregisterHit(AttackSubManager attack)
    {
        atkRegistered.Remove(attack);
    }

    public void CheckHit(CharacterBase character)
    {
        for (int i = atkRegistered.Count-1; i >= 0; i--)
        {
            if (Parry.CanParry(atkRegistered[i]) == true)   // On parry
            {
                Parry.ParryResolution(character, atkRegistered[i]);
            }
            else if (Parry.CanGuard(atkRegistered[i]) == true)   // On Garde
            {
                Parry.GuardResolution(character, atkRegistered[i]);
            }
            else if (atkRegistered[i].AttackClashed != null) // On clash
            {
                Parry.Clash(character, atkRegistered[i]); // Le clash
                atkRegistered[i].User.Knockback.UnregisterHit(atkRegistered[i].AttackClashed); // On retire l'attaque de l'adversaire pour ne pas lancer 2 fois le clash
            }
            else if (Parry.CanJustFrameParry(character, atkRegistered[i])) // Parry just frame, on doit check que le bouton R1 est appuyé et que le perso est dans un état om il est possible de parer
            {
                Debug.Log("Just frame");
                Parry.ParryResolution(character, atkRegistered[i]);
            }
            else // On touche
            {
                Hit(character, atkRegistered[i]);
            }
            atkRegistered.RemoveAt(i);
        }
        character.Knockback.Parry.IsJustFrameParry = false;

    }


    public void Hit(CharacterBase character, AttackSubManager attack)
    {
        attack.Hit(character);
        if (CanKnockback() == true)
            character.SetState(stateKnockback);
        OnKnockback?.Invoke(attack);
    }



    public Vector2 GetAngleKnockback()
    {
        return angleKnockback;
    }

    /// <summary>
    /// Launch utilisé par le knockback
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="ejectionPower"></param>
    /// <param name="bonusKnockback"></param>
    public void Launch(Vector2 angle, float ejectionPower, float bonusKnockback = 0)
    {
        if (isArmor == true)
            return;
        angleKnockback = angle * weight;
        angleKnockback *= ejectionPower; // (damagePercentage / damagePercentageRatio);

        if (knockbackDuration > 0)
            knockbackDuration = knockbackDuration * knockbackAcumulationModifier;
        else
            knockbackDuration = 0;

        knockbackDuration += timeKnockbackPerDistance * angleKnockback.magnitude;
        knockbackDuration = Mathf.Clamp(knockbackDuration, 0, maxTimeKnockback);
        knockbackDuration += bonusKnockback;
    }

    /// <summary>
    /// Launch arbitraire
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="ejectionPower"></param>
    public void Launch(Vector2 angle, float ejectionPower)
    {
        angleKnockback = angle * weight;
        angleKnockback *= ejectionPower;
    }

    public void UpdateKnockback(float percentage)
    {
        knockbackDuration -= (Time.deltaTime * percentage) * motionSpeed;
    }

}
