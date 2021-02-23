using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateStartJump : CharacterState
{
	[SerializeField]
	private CharacterState jumpState;

	[SerializeField]
	private Animator animator;

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	public override void StartState(CharacterBase character, CharacterState oldState)
	{

	}

	public override void UpdateState(CharacterBase character)
	{
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ProtoMan_StartJump"))
        {
			Debug.Log("Jump");
			character.Movement.Jump();
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