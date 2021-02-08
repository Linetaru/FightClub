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
	CharacterState wallRunState;
	[SerializeField]
	CharacterMovement movement;

	[SerializeField]
	int numberOfAerialJump = 1;

	[SerializeField]
	[ReadOnly] int currentNumberOfAerialJump = 1;

	[SerializeField]
	float jumpForce = 10f;

	[SerializeField]
	float gravity = 1f;

	// Start is called before the first frame update
	void Start()
	{
		currentNumberOfAerialJump = numberOfAerialJump;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public override void StartState(CharacterBase character)
	{
		Debug.Log("AerialState");
		//if (currentNumberOfJump == 0 && characterRigidbody.IsGrounded)
		//{
		//	currentNumberOfJump = numberOfJump;
		//}

		//if (currentNumberOfJump > 0)
		//{
		//	currentNumberOfJump--;
		//	movement.SpeedY = jumpForce;
		//}
	}

	public override void UpdateState(CharacterBase character)
	{
		if (character.Input.inputActions.Count != 0 && !characterRigidbody.IsGrounded && currentNumberOfAerialJump > 0 && movement.SpeedY < 0)
		{
			if (character.Input.inputActions[0].action == InputConst.Jump)
			{
				currentNumberOfAerialJump--;
				movement.Jump(jumpForce);
				character.Input.inputActions[0].timeValue = 0;
			}
		}
		else if (movement.SpeedY < (jumpForce / 2) && characterRigidbody.IsGrounded)
		{
			character.SetState(idleState);
			return;
		}

		characterRigidbody.UpdateCollision(movement.SpeedX * movement.Direction, movement.SpeedY);

		if (characterRigidbody.CollisionWallInfo != null && movement.SpeedY >= 0 && Mathf.Abs(movement.SpeedX) > 5)
		{
			character.SetState(wallRunState);
			//wallrunCount = 0;
			return;
		}
			GravityChange();
	}

	public override void EndState(CharacterBase character)
	{
		if (currentNumberOfAerialJump == 0 && characterRigidbody.IsGrounded)
		{
			currentNumberOfAerialJump = numberOfAerialJump;
		}
	}

	public void GravityChange()
	{
		movement.SpeedY -= gravity;
	}
}