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
    float baseWallRunSpeed = 8.0f;
    [SerializeField]
    float wallrunSpeedMax = 10.0f;
    [SerializeField]
    float wallrunSpeedMin = -2.0f;

    [SerializeField]
    float wallJumpSpeedX = 5.0f;

    float horizontalDeadZone = .1f;

    bool wallCollision = false;
    bool groundCollision = false;
    
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
        Debug.Log("Wallrun");
        Debug.Log(character.Rigidbody.IsGrounded);
        groundCollision = Physics.Raycast(transform.position, Vector3.down, out _, 1f, wallLayer);

        character.Movement.Direction = (int)Mathf.Sign(character.Movement.SpeedX * character.Movement.Direction);
        float speedXBeforeWallRun = character.Movement.SpeedX;

        wallrunSpeed = baseWallRunSpeed + speedXBeforeWallRun/4;

        if (wallrunSpeed > wallrunSpeedMax)
            wallrunSpeed = wallrunSpeedMax;

        if (character.Rigidbody.IsGrounded)
        {
            wallrunSpeed = wallrunSpeedMax;
            character.Movement.SetSpeed(0.0f, wallrunSpeed/* + speedXBeforeWallRun*/);
        }
        else
        {
            wallrunSpeed = wallrunSpeedMax;
            character.Movement.SetSpeed(0.0f, wallrunSpeed/* + speedXBeforeWallRun*/);
        }
    }

    public override void UpdateState(CharacterBase character)
    {
        wallCollision = (Physics.Raycast(transform.position, Vector3.right * character.Movement.Direction, out _, .3f, wallLayer));
        groundCollision = Physics.Raycast(transform.position, Vector3.down, out _, 1f, wallLayer);
        
            if (character.Movement.SpeedY > wallrunSpeedMin)
            {
                wallrunSpeed -= deccelerationRate * Time.deltaTime;
                character.Movement.SpeedY = wallrunSpeed;
            }
            else
            {
                character.Movement.SpeedY = wallrunSpeedMin;
            }

            if (character.Input.inputActions.Count != 0 && wallCollision)
            {
                if (character.Input.inputActions[0].action == InputConst.Jump)
                {
                    character.Movement.Direction = character.Movement.Direction * -1;

                    if (character.Movement.SpeedY > 0)
                        character.Movement.SpeedX = wallJumpSpeedX + character.Movement.SpeedY;
                    else
                        character.Movement.SpeedX = wallJumpSpeedX;

                    wallCollision = false;

                    character.Movement.Jump();

                    //Play Walljump animation

                    character.SetState(aerialState);
                    character.Input.inputActions[0].timeValue = 0;
                }
            }
        

        //if (Mathf.Abs(character.Input.horizontal) > horizontalDeadZone && Mathf.Sign(character.Input.horizontal) == character.Movement.Direction && wallCollision)
        //{
        //}
        //else
        //{
        //    if (character.Rigidbody.IsGrounded)
        //    {
        //        character.SetState(idleState);
        //    }
        //    else
        //    {
        //        character.SetState(aerialState);
        //    }
        //}
    }

    public override void LateUpdateState(CharacterBase character)
    {
        base.LateUpdateState(character);

        if (character.Rigidbody.IsGrounded)
        {
            character.SetState(idleState);
        }
        else if (!wallCollision)
        {
            character.SetState(aerialState);
        }
    }

    public override void EndState(CharacterBase character, CharacterState oldState)
    {

            Debug.Log("Wallrun end");
    }



    public void JumpWallRun()
    {
    }

}