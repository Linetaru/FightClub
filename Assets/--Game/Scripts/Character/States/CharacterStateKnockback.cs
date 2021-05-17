using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateKnockback : CharacterState
{

    [SerializeField]
    CharacterState groundTechState;
    [SerializeField]
    CharacterState airTechState;

    [Title("Parameter - Collision")]
    [SerializeField]
    float collisionFriction = 5f;
    [SerializeField]
    [Range(0,1)]
    float reboundReduction = 0.75f;

    [SerializeField]
    float reboundSpeedNeeded = 2f;

    [SerializeField]
    [SuffixLabel("en frames")]
    float landingTime = 10;

    [Title("Parameter - Collision")]
    [SerializeField]
    LayerMask knockbackLayerMask;

    [Title("Parameter - Collision")]
    [SerializeField]
    ParticleSystem particleTrail;

    [Title("Parameter - DI")]
    [SerializeField]
    float joystickThreshold = 0.3f;
    [SerializeField]
    float DIAngle = 10;

    [Title("Parameter - Actions")]
    [SerializeField]
    CharacterAcumods acumods;

    bool inHitStop = true;
    float tech = 0;
    float techCooldown = 0;

    private void Start()
    {
        landingTime /= 60f;
    }

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        inHitStop = true;
        particleTrail.Play();
        character.Action.CancelAction();
        character.Movement.SpeedX = character.Knockback.GetAngleKnockback().x;
        character.Movement.SpeedX *= character.Movement.Direction;
        character.Movement.SpeedY = character.Knockback.GetAngleKnockback().y;

        character.Rigidbody.SetNewLayerMask(knockbackLayerMask, false, true);
        character.Rigidbody.SetNewLayerMask(knockbackLayerMask);

        character.Knockback.Parry.ParryNumber = 0;
        tech = 0f;
        techCooldown = 0f;
    }

    public override void UpdateState(CharacterBase character)
    {
        if(inHitStop == true && character.MotionSpeed != 0)
        {
            DirectionalInfluence(character);
            inHitStop = false;
        }

        if (Mathf.Abs(character.Movement.SpeedX) < (collisionFriction * Time.deltaTime * 2))
            character.Movement.SpeedX = 0;
        else
            character.Movement.SpeedX -= (collisionFriction * Mathf.Sign(character.Movement.SpeedX)) *  Time.deltaTime;



        character.Movement.ApplyGravity();


        character.Knockback.UpdateKnockback(1);
        if (character.Knockback.KnockbackDuration <= 0)
        {
            character.ResetToIdle();
        }
        else if (character.Input.CheckAction(0, InputConst.RightTrigger) && techCooldown <= 0)
        {
            tech = 6 / 60f;
            techCooldown = 20 / 60f;
        }
        else if (acumods.Acumod(character))
        {

        }

        tech -= Time.deltaTime;
        techCooldown -= Time.deltaTime;

    }

    public override void LateUpdateState(CharacterBase character)
    {
        if ((character.Rigidbody.CollisionGroundInfo != null || character.Rigidbody.CollisionRoofInfo != null) && Mathf.Abs(character.Movement.SpeedY) > reboundSpeedNeeded)
        {
            if (tech >= 0 && character.Movement.SpeedY < 0)
                character.SetState(groundTechState);
            else
                character.Movement.SpeedY = -character.Movement.SpeedY * reboundReduction;
            //Feedbacks.GlobalFeedback.Instance.SuperFeedback(); // A degager peut etre

        }

        if (character.Rigidbody.CollisionWallInfo.Collision != null)
        {
            if (tech >= 0)
            {
                character.SetState(airTechState);
                return;
            }
            character.Movement.SpeedX = -character.Movement.SpeedX * reboundReduction;
            //Feedbacks.GlobalFeedback.Instance.SuperFeedback(); // A degager peut etre
        }
    }


    private void DirectionalInfluence(CharacterBase character)
    {
        if (Mathf.Abs(character.Input.horizontal) < joystickThreshold && Mathf.Abs(character.Input.vertical) < joystickThreshold)
            return;

        float power = character.Knockback.GetAngleKnockback().magnitude;
        Vector2 ejectionAngle = character.Knockback.GetAngleKnockback().normalized;
        Vector2 input = new Vector2(character.Input.horizontal, character.Input.vertical);

        float influence = Vector2.Dot(input, Vector2.Perpendicular(ejectionAngle));

        Vector2 finalDirection = Quaternion.Euler(0, 0, DIAngle * influence) * ejectionAngle;
        character.Knockback.Launch(finalDirection.normalized, power);

        character.Movement.SpeedX = character.Knockback.GetAngleKnockback().x;
        character.Movement.SpeedX *= character.Movement.Direction;
        character.Movement.SpeedY = character.Knockback.GetAngleKnockback().y;
    }


    public override void EndState(CharacterBase character, CharacterState oldState)
    {
        particleTrail.Stop();
        character.Rigidbody.ResetLayerMask();
    }
}