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

    [Title("FeedbackComponents")]
    [SerializeField]
    private ShakeEffect shakeEffect;
    public ShakeEffect ShakeEffect
    {
        get { return shakeEffect; }
    }

    //================================================================================

    [Title("Parameter")]
    [SerializeField]
    private float weight = 1;
    public float Weight
    {
        get { return weight; }
    }

    [SerializeField]
    private float timeKnockbackPerDistance;
    public float TimeKnockbackPerDistance
    {
        get { return timeKnockbackPerDistance; }
    }


    /*[Title("Parameter - (Ptet a bouger)")]
    [SerializeField]
    private float damagePercentageRatio = 150f;
    public float DamagePercentageRatio
    {
        get { return damagePercentageRatio; }
    }*/



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


    // A mettre dans une interface
    public bool CanKnockback()
    {
        if (knockbackDuration <= 0)
            return false;
        return true;
    }

    // Fonction a appelé quand le personnage se fait touché
    // A mettre dans une interface
    public void Hit()
    {
        // Event onHit
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
