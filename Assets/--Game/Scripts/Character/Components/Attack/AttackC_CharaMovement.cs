using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


// Ptet a bouger pour les states machines pour chaque action ?
public class AttackC_CharaMovement : AttackComponent
{
    [Title("Movement X")]
    [SerializeField]
    bool keepMomentumX = false;


    [HideIf("keepMomentumX")]
    [HorizontalGroup("MomentumX")]
    [SerializeField]
    bool setMomentumX = false;

    [HorizontalGroup("MomentumX")]
    [HideIf("keepMomentumX")]
    [SerializeField]
    float momentumX = 0f;




    //[ShowIf("keepMomentumX")]
    [SerializeField]
    bool deccelerate = false;

    [HorizontalGroup("Decceleration")]
    [ShowIf("deccelerate")]
    [SerializeField]
    [HideLabel]
    AnimationCurve deccelerationCurve;

    [HorizontalGroup("Decceleration", LabelWidth = 120)]
    [ShowIf("deccelerate")]
    [SuffixLabel("en frames")]
    [SerializeField]
    float timeDecceleration = 10f;




    [Title("Movement Y")]
    [SerializeField]
    bool keepMomentumY = false;

    [HorizontalGroup("Gravity")]
    [SerializeField]
    bool applyGravity = false;

    [HorizontalGroup("Gravity")]
    [ShowIf("applyGravity")]
    [SerializeField]
    float gravityMultiplier = 1f;

    [HorizontalGroup("GravityDescend")]
    [SerializeField]
    bool applyGravityDescend = false;

    [HorizontalGroup("GravityDescend")]
    [ShowIf("applyGravityDescend")]
    [SerializeField]
    float gravityMultiplierDescend = 1f;

    [SerializeField]
    bool canAirControl = false;



    [Title("Ground Cancel")]
    [SerializeField]
    bool groundCancel = false;
    [SerializeField]
    [ShowIf("groundCancel")]
    [Tooltip("If not specified, return to idle")]
    private AttackManager groundEndLag;



    float timer = 0;
    float initialSpeedX = 0;

    bool groundCancelNextFrame = false;


    public override void StartComponent(CharacterBase user)
    {
        if (keepMomentumX == false)
        {
            user.Movement.SpeedX = 0;
            if(setMomentumX == true)
                user.Movement.SpeedX = momentumX;
        }

        if (keepMomentumY == false)
            user.Movement.SpeedY = 0;

        timer = 0f;
        initialSpeedX = user.Movement.SpeedX;
        timeDecceleration /= 60f;

    }

    // jsp
    public override void UpdateComponent(CharacterBase user)
    {

        if (applyGravity == true)
        {
            if(applyGravityDescend && user.Movement.SpeedY < 0)
                user.Movement.ApplyGravity(gravityMultiplierDescend);
            else
                user.Movement.ApplyGravity(gravityMultiplier);
        }

        if (deccelerate == true && timer < timeDecceleration)
        {
            timer += Time.deltaTime * user.MotionSpeed;
            user.Movement.SpeedX = initialSpeedX * deccelerationCurve.Evaluate(timer / timeDecceleration);
            if (timer >= timeDecceleration)
                user.Movement.SpeedX = 0;
        }

      /*  if (accelerate == true && timer < timeDecceleration)
        {
            timer += Time.deltaTime * user.MotionSpeed;
            user.Movement.SpeedX = initialSpeedX * AccelerationCurve.Evaluate(timer / timeAcceleration);
            if (timer >= timeAcceleration)
                user.Movement.SpeedX = 0;
        }*/

        if (canAirControl == true)
            user.Movement.AirControl(user.Input.horizontal);



        if(groundCancelNextFrame == true)
        {
            user.Action.CancelAction();
            user.ResetToLand();
        }

        if (groundCancel == true && user.Rigidbody.CollisionGroundInfo != null)
        {
            if (groundEndLag != null)
            {
                user.Action.CancelAction();
                user.Action.Action(groundEndLag);
            }
            else
            {
                groundCancelNextFrame = true;
            }
        }

    }


    public override void OnHit(CharacterBase user, CharacterBase target)
    {

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
    public override void EndComponent(CharacterBase user)
    {

    }
}
