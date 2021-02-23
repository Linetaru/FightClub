using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateKnockback : CharacterState
{
    [SerializeField]
    CharacterState idleState;
    [SerializeField]
    CharacterState aerialState;

    [SerializeField]
    CharacterRigidbody characterRigidbody;
    [SerializeField]
    CharacterMovement movement;

    [Header("Knockback Stats")]
    [SerializeField]
    float minimalKnockBackSpeed = 4.0f;
    [SerializeField]
    float collisionFriction = 0.8f;

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

    }

    public override void UpdateState(CharacterBase character)
    {
        character.Knockback.UpdateKnockback(character.Stats.LifePercentage);

        if (Mathf.Abs(character.Knockback.GetAngleKnockback().magnitude) < minimalKnockBackSpeed)
        {
            if (characterRigidbody.IsGrounded)
            {
                character.SetState(idleState);
                //knockbackDuration = 0f;
            }
            else
            {
                character.SetState(aerialState);
                //knockbackDuration = 0f;
            }
        }

        character.Movement.SpeedX = character.Knockback.GetAngleKnockback().x;
        character.Movement.SpeedY = character.Knockback.GetAngleKnockback().y;

        if(movement.SpeedX > 0)
        movement.SpeedX -= Time.deltaTime;
        else
        movement.SpeedX += Time.deltaTime;

        movement.SpeedY -= Time.deltaTime;
        character.Movement.ApplyGravity();
    }

    public override void LateUpdateState(CharacterBase character)
    {
        if (characterRigidbody.CollisionGroundInfo != null || characterRigidbody.CollisionRoofInfo != null)
        {
            //movement.SpeedY = -(movement.SpeedY * collisionFriction);
            character.Knockback.Launch(new Vector2(character.Knockback.GetAngleKnockback().x, -character.Knockback.GetAngleKnockback().y));
        }

        if (characterRigidbody.CollisionWallInfo != null)
        {
            //movement.SpeedX = -(movement.SpeedX * collisionFriction);
            character.Knockback.Launch(new Vector2(-character.Knockback.GetAngleKnockback().x, character.Knockback.GetAngleKnockback().y));
        }
    }

    public override void EndState(CharacterBase character, CharacterState oldState)
    {

    }
}