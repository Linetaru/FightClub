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

    [SerializeField]
    float minimalKnockBackSpeed = 4.0f;
    [SerializeField]
    float collisionFriction = 0.8f;

    [SerializeField]
    float knockbackDuration = 0;

    Vector2 projectionAngle = new Vector2(0f, 0f);

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
        projectionAngle = new Vector2(movement.SpeedX, movement.SpeedY);
    }

    public override void UpdateState(CharacterBase character)
    {
        if (Mathf.Abs(projectionAngle.magnitude) < minimalKnockBackSpeed)
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
    }

    public override void LateUpdateState(CharacterBase character)
    {
        if (characterRigidbody.CollisionGroundInfo != null || characterRigidbody.CollisionRoofInfo != null)
        {
            movement.SpeedY = -(movement.SpeedY * collisionFriction);
        }

        if (character.Rigidbody.CollisionGroundInfo != null)
        {
            movement.SpeedX = -(movement.SpeedX * collisionFriction);
        }
    }

    public override void EndState(CharacterBase character, CharacterState oldState)
    {

    }
}