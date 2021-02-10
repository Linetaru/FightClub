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
    float minimalKnockBackSpeed = 2.0f;

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

    public override void StartState(CharacterBase character)
    {

    }

    public override void UpdateState(CharacterBase character)
    {
        if (Mathf.Abs(movement.SpeedX) < minimalKnockBackSpeed && Mathf.Abs(movement.SpeedY) < minimalKnockBackSpeed)
        {
            if (characterRigidbody.IsGrounded)
            {
                character.SetState(idleState);
                knockbackDuration = 0f;
            }
            else
            {
                character.SetState(aerialState);
                knockbackDuration = 0f;
            }
        }
    }

    public override void EndState(CharacterBase character)
    {

    }
}