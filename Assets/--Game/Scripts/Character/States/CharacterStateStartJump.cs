using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateStartJump : CharacterState
{
    [SerializeField]
    private CharacterState jumpState;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float shortJumpForce;
    public float ShortJumpForce
    {
        get { return shortJumpForce; }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        character.Movement.SpeedY = 0;
    }

    public override void UpdateState(CharacterBase character)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ProtoMan_StartJump"))
        {
            Debug.Log("Jump");
            if(Input.GetButton("Fire2"))
                character.Movement.Jump();
            else
                character.Movement.Jump(shortJumpForce);

                character.SetState(jumpState);
        }
    }

    public override void LateUpdateState(CharacterBase character)
    {

    }

    public override void EndState(CharacterBase character, CharacterState newState)
    {

    }
}