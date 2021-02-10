using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateWallRun : CharacterState
{

    [SerializeField]
    CharacterState idleState;
    [SerializeField]
    CharacterState aerialState;


    [SerializeField]
    float stickRunThreshold = 0.7f;
    [SerializeField]
    float deccelerationRate = 0.7f;

    float wallrunSpeed = 10.0f;
    [SerializeField]
    float wallrunSpeedMax = 10.0f;
    [SerializeField]
    float wallrunSpeedMin = -2.0f;

    [SerializeField]
    float wallJumpSpeedX = 5.0f;

    float horizontalDeadZone = .1f;

    bool wallCollision = false;
    
    [SerializeField]
    LayerMask wallLayer;


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
        //wallCollision = true;
        Debug.Log("Wallrun");
        Debug.Log(character.Movement.SpeedX);

        float speedXBeforeWallRun = character.Movement.SpeedX;


        Debug.Log(speedXBeforeWallRun);

        wallrunSpeed = wallrunSpeedMax + speedXBeforeWallRun;
        if (character.Movement.SpeedX > 0)
        {

            character.Movement.SetSpeed(0.0f, wallrunSpeed/* + speedXBeforeWallRun*/);
        }
        else
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

    public override void UpdateState(CharacterBase character)
    {
        wallCollision = (Physics.Raycast(transform.position, Vector3.right * character.Movement.Direction, out _, 1.0f, wallLayer));
        
        if (Mathf.Abs(character.Input.horizontal) > horizontalDeadZone && Mathf.Sign(character.Input.horizontal) == character.Movement.Direction && wallCollision)
        {
            if (character.Movement.SpeedY > wallrunSpeedMin)
            {
                wallrunSpeed -= deccelerationRate * Time.deltaTime;
                character.Movement.SpeedY = wallrunSpeed;
            }
            else
            {
                character.Movement.SpeedY = wallrunSpeedMin;
            }

            if (character.Movement.SpeedY > 0)
            {
                //Play Wallrun animation
            }
            else
            {
                //Play WallSlide Animation (rester accroché au mur tout en se laissant tomber)
            }

            if (character.Input.inputActions.Count != 0 && wallCollision)
            {
                if (character.Input.inputActions[0].action == InputConst.Jump)
                {
                    if (character.Movement.SpeedY > 0)
                        character.Movement.SpeedX = wallJumpSpeedX + character.Movement.SpeedY;
                    else
                        character.Movement.SpeedX = wallJumpSpeedX;
                    character.Movement.Direction = character.Movement.Direction * -1;
                    character.Movement.Jump();

                    //Play Walljump animation

                    character.SetState(aerialState);
                    character.Input.inputActions[0].timeValue = 0;
                    wallCollision = false;
                }
            }
        }
        else
        {
            Debug.Log("IS NO MORE IN WALLRUN");
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

    public override void EndState(CharacterBase character, CharacterState oldState)
    {

    }



    public void JumpWallRun()
    {
    }

}