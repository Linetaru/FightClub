﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateKnockback : CharacterState
{
    [SerializeField]
    CharacterState idleState;
    [SerializeField]
    CharacterState aerialState;


    [Title("Parameter - Collision")]
    [SerializeField]
    float collisionFriction = 5f;
    [SerializeField]
    [MaxValue(1)]
    [MinValue(0)]
    float reboundReduction = 0.75f;

    [Title("Parameter - Collision")]
    [SerializeField]
    LayerMask knockbackLayerMask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        character.Action.CancelAction();
        character.Movement.SpeedX = character.Knockback.GetAngleKnockback().x;
        character.Movement.SpeedY = character.Knockback.GetAngleKnockback().y;
        character.Rigidbody.SetNewLayerMask(knockbackLayerMask);
    }

    public override void UpdateState(CharacterBase character)
    {
        if (Mathf.Abs(character.Movement.SpeedX) < (collisionFriction * Time.deltaTime * 2))
            character.Movement.SpeedX = 0;
        else
            character.Movement.SpeedX -= (collisionFriction * Mathf.Sign(character.Movement.SpeedX)) *  Time.deltaTime;

        character.Movement.ApplyGravity();


        character.Knockback.UpdateKnockback(1);
        if (character.Knockback.KnockbackDuration <= 0)
        {
            if (character.Rigidbody.IsGrounded)
            {
                character.SetState(idleState);
            }
            else
            {
                character.SetState(aerialState);
            }
        }

    }

    public override void LateUpdateState(CharacterBase character)
    {
        if (character.Rigidbody.CollisionGroundInfo != null || character.Rigidbody.CollisionRoofInfo != null)
        {
            character.Movement.SpeedY = -character.Movement.SpeedY * reboundReduction;
        }

        if (character.Rigidbody.CollisionWallInfo != null)
        {
            character.Movement.SpeedX = -character.Movement.SpeedX * reboundReduction;
        }
    }

    public override void EndState(CharacterBase character, CharacterState oldState)
    {
        character.Rigidbody.ResetLayerMask();
    }
}