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
    CharacterState stateKnockback;


    [Title("Parry")]
    [SerializeField]
    private CharacterParry parry;
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
    private float timeKnockbackPerDistance;
    public float TimeKnockbackPerDistance
    {
        get { return timeKnockbackPerDistance; }
    }

    [SerializeField]
    private float maxTimeKnockback = 1.5f;
    public float MaxTimeKnockback
    {
        get { return maxTimeKnockback; }
    }


    private Vector2 angleKnockback;

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


    protected float motionSpeed = 1;
    public float MotionSpeed
    {
        get { return motionSpeed; }
        set { motionSpeed = value; }
    }



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
                /*Parry.Parry(character, atkRegistered[i].User);
                atkRegistered[i].User.Knockback.Parry.ParryRepel(atkRegistered[i].User, character);
                atkRegistered[i].AddPlayerHitList(character.tag);

                // Pour tourner le joueur dans le sens de la parry
                if (Mathf.Sign(atkRegistered[i].User.transform.position.x - character.transform.position.x) != character.Movement.Direction)
                    character.Movement.Direction *= -1;*/
            }
            else if (Parry.CanGuard(atkRegistered[i]) == true)   // On Garde
            {
                Parry.GuardResolution(character, atkRegistered[i]);
               /* atkRegistered[i].User.Knockback.ContactPoint = character.Knockback.ContactPoint;

                Parry.Guard(character, atkRegistered[i].User);
                atkRegistered[i].User.Knockback.Parry.Parry(atkRegistered[i].User, character);
                atkRegistered[i].User.PowerGauge.ForceAddPower(-25);
                atkRegistered[i].AddPlayerHitList(character.tag);

                // Pour tourner le joueur dans le sens de la garde
                if (Mathf.Sign(atkRegistered[i].User.transform.position.x - character.transform.position.x) != character.Movement.Direction)
                    character.Movement.Direction *= -1;*/
            }
            else if(atkRegistered[i].AttackClashed != null) // On clash
            {
                Parry.Clash(character, atkRegistered[i]); // Le clash

                atkRegistered[i].User.Knockback.UnregisterHit(atkRegistered[i].AttackClashed); // On retire l'attaque de l'adversaire pour ne pas lancer 2 fois le clash
            }
            else // On touche
            {
                Hit(character, atkRegistered[i]);
                /*atkRegistered[i].Hit(character);
                if (CanKnockback() == true)
                    character.SetState(stateKnockback);
                OnKnockback?.Invoke(atkRegistered[i]);*/
            }
            atkRegistered.RemoveAt(i);
        }
 
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

        knockbackDuration = timeKnockbackPerDistance * angleKnockback.magnitude;
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
        knockbackDuration -= Time.deltaTime * motionSpeed;
    }
}
