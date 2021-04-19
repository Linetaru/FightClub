using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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

    float joystickDeadzone = .9f;

    bool wallCollision = false;
    bool groundCollision = false;

    [SerializeField]
    LayerMask wallLayer;

    // Start is called before the first frame update
    /*void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }*/

    public override void StartState(CharacterBase character, CharacterState oldState)
    {

        character.Movement.Direction = (int)Mathf.Sign(character.Movement.SpeedX * character.Movement.Direction);

        wallrunSpeed = Mathf.Clamp(character.Movement.SpeedX, wallrunSpeedMin, wallrunSpeedMax);
        character.Movement.SetSpeed(0.0f, wallrunSpeed);

        character.PowerGauge.canGainPointByWallRun = true;

        character.Movement.CurrentNumberOfJump = character.Movement.JumpNumber;
    }

    public override void UpdateState(CharacterBase character)
    {
        wallCollision = (Physics.Raycast(transform.position, Vector3.right * character.Movement.Direction, out _, .3f, wallLayer));
        //groundCollision = Physics.Raycast(transform.position, Vector3.down, out _, 1f, wallLayer);

        if (character.Movement.SpeedY > wallrunSpeedMin)
        {
            wallrunSpeed -= deccelerationRate * Time.deltaTime;
            if (character.Input.vertical < -joystickDeadzone) 
                wallrunSpeed -= (deccelerationRate * 3) * Time.deltaTime;
            character.Movement.SpeedY = wallrunSpeed;
        }
        else
        {
            character.Movement.SpeedY = wallrunSpeedMin;
            if (Mathf.Abs(character.Input.horizontal) > joystickDeadzone && Mathf.Sign(character.Input.horizontal) != character.Movement.Direction) //|| character.Input.vertical < -joystickDeadzone)
            {
                character.SetState(aerialState);
                //character.Input.inputActions[0].timeValue = 0;
            }
        }

        if (character.Input.inputActions.Count != 0 && wallCollision)
        {
            if (character.Input.inputActions[0].action == InputConst.Jump || character.Input.CheckAction(0, InputConst.Smash))
            {
                character.Movement.Direction *= -1;

                /*if (character.Movement.SpeedY > 0)
                    character.Movement.SpeedX = wallJumpSpeedX + character.Movement.SpeedY;
                else*/
                character.Movement.SpeedX = wallJumpSpeedX;

                wallCollision = false;

                character.Movement.Jump(); 
                ParticleSystem particle = Instantiate(character.Particle.startJumpParticle, this.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(character.Movement.SpeedX * character.Movement.Direction, character.Movement.SpeedY) * Mathf.Rad2Deg));
                Destroy(particle.gameObject, 0.5f);
                character.PowerGauge.AddPower(character.PowerGauge.powerGivenOnWallJump);

                //Play Walljump animation

                character.SetState(aerialState);
                character.Input.inputActions[0].timeValue = 0;
            }
        }

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
        character.PowerGauge.canGainPointByWallRun = false;
    }



    public void JumpWallRun()
    {
    }

}