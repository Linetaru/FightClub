using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateKnockback : CharacterState
{
    [SerializeField]
    CharacterState idleState;
    [SerializeField]
    CharacterState landState;
    [SerializeField]
    CharacterState aerialState;


    [Title("Parameter - Collision")]
    [SerializeField]
    float collisionFriction = 5f;
    [SerializeField]
    [Range(0,1)]
    float reboundReduction = 0.75f;

    [SerializeField]
    [SuffixLabel("en frames")]
    float landingTime = 10;

    [Title("Parameter - Collision")]
    [SerializeField]
    LayerMask knockbackLayerMask;

    [Title("Parameter - Collision")]
    [SerializeField]
    ParticleSystem particleTrail;


    private void Start()
    {
        landingTime /= 60f;
    }

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        particleTrail.Play();
        character.Action.CancelAction();
        character.Movement.SpeedX = character.Knockback.GetAngleKnockback().x;
        character.Movement.SpeedX *= character.Movement.Direction;
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
            character.ResetToIdle();
            /*if (character.Rigidbody.IsGrounded)
            {
                character.SetState(idleState);
            }
            else
            {
                character.SetState(aerialState);
            }*/
        }

    }

    public override void LateUpdateState(CharacterBase character)
    {
        if (character.Rigidbody.CollisionGroundInfo != null || character.Rigidbody.CollisionRoofInfo != null)
        {
            character.Movement.SpeedY = -character.Movement.SpeedY * reboundReduction;
            if (character.Rigidbody.CollisionGroundInfo != null && character.Knockback.KnockbackDuration <= landingTime)
            {
                character.SetState(landState);
            }
        }

        if (character.Rigidbody.CollisionWallInfo != null)
        {
            character.Movement.SpeedX = -character.Movement.SpeedX * reboundReduction;
        }
    }

    public override void EndState(CharacterBase character, CharacterState oldState)
    {
        particleTrail.Stop();
        character.Rigidbody.ResetLayerMask();
    }
}