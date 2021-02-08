using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateAerial : CharacterState
{

	[SerializeField]
	CharacterRigidbody characterRigidbody;
	[SerializeField]
	CharacterState idleState;
	[SerializeField]
	CharacterMovement movement;

	[SerializeField]
	int numberOfJump = 1;

	[SerializeField]
	[ReadOnly] int currentNumberOfJump = 1;

	[SerializeField]
	float jumpForce = 10f;

	[SerializeField]
	float gravity = 1f;

	// Start is called before the first frame update
	void Start()
	{
		currentNumberOfJump = numberOfJump;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public override void StartState(CharacterBase character)
	{
		//if (currentNumberOfJump == 0 && characterRigidbody.IsGrounded)
		//{
		//	currentNumberOfJump = numberOfJump;
		//}

		if (currentNumberOfJump > 0)
		{
			currentNumberOfJump--;
			movement.SpeedY = jumpForce;
		}
	}

	public override void UpdateState(CharacterBase character)
	{
		if (character.Input.inputActions.Count != 0 && !characterRigidbody.IsGrounded && currentNumberOfJump > 0 && movement.SpeedY < 0)
		{
			if (character.Input.inputActions[0].action == InputConst.Jump)
			{
				currentNumberOfJump--;
				movement.SpeedY = jumpForce;
				character.Input.inputActions[0].timeValue = 0;
			}
		}
		else if (movement.SpeedY < (jumpForce / 2) && characterRigidbody.IsGrounded)
			character.SetState(idleState);

		characterRigidbody.UpdateCollision(movement.SpeedX * movement.Direction, movement.SpeedY);
		GravityChange();
	}

	public override void EndState(CharacterBase character)
	{
		if (currentNumberOfJump == 0 && characterRigidbody.IsGrounded)
		{
			currentNumberOfJump = numberOfJump;
		}
	}

	public void GravityChange()
	{
		movement.SpeedY -= gravity;
	}
}