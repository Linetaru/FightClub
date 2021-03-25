﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


// Ptet a bouger pour les states machines pour chaque action ?
public class AttackC_CharaMovement : AttackComponent
{
    [Title("Movement X")]
    [SerializeField]
    bool keepMomentumX = false;

    [ShowIf("keepMomentumX")]
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



    [Title("Ground Cancel")]
    [SerializeField]
    bool groundCancel = false;
    [SerializeField]
    [ShowIf("groundCancel")]
    [Tooltip("If not specified, return to idle")]
    private AttackManager groundEndLag;



    float timer = 0;
    float initialSpeedX = 0;


    public override void StartComponent(CharacterBase user)
    {
        if (keepMomentumX == false)
            user.Movement.SpeedX = 0;
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
            user.Movement.ApplyGravity(gravityMultiplier);
        }

        if (deccelerate == true && timer < timeDecceleration)
        {
            timer += Time.deltaTime * user.MotionSpeed;
            user.Movement.SpeedX = initialSpeedX * deccelerationCurve.Evaluate(timer / timeDecceleration);
            if (timer >= timeDecceleration)
                user.Movement.SpeedX = 0;
        }



        if (groundCancel == true && user.Rigidbody.CollisionGroundInfo != null)
        {
            if(groundEndLag != null)
            {
                user.Action.CancelAction();
                user.Action.Action(groundEndLag);
            }
            else
            {
                user.Action.EndAction();
            }
        }

    }


    public override void OnHit(CharacterBase user, CharacterBase target)
    {

    }
    public override void EndComponent(CharacterBase user)
    {

    }
}
