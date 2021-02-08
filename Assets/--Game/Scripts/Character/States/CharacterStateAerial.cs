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
	float minimalSpeedToWallRun = -2;

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
		if (character.Input.inputActions.Count != 0 && currentNumberOfAerialJump > 0)
		{
			if (character.Input.inputActions[0].action == InputConst.Jump)
			{
				currentNumberOfAerialJump--;
				movement.Jump(jumpForce);
				character.Input.inputActions[0].timeValue = 0;
			}
		}
		GravityChange();
		characterRigidbody.UpdateCollision(movement.SpeedX * movement.Direction, movement.SpeedY);


		if (characterRigidbody.CollisionGroundInfo != null)
		{
			character.SetState(idleState);
			return;
		}
		if (characterRigidbody.CollisionWallInfo != null && Mathf.Abs(movement.SpeedX) > 2)
		{
			character.SetState(wallRunState);
			//wallrunCount = 0;
			return;
		}
	}

	public override void EndState(CharacterBase character)
	{
		currentNumberOfAerialJump = numberOfAerialJump;
		/*if (currentNumberOfAerialJump == 0 && characterRigidbody.IsGrounded)
		{
			currentNumberOfAerialJump = numberOfAerialJump;
		}*/
	}

	public void GravityChange()
	{
		movement.SpeedY -= gravity * Time.deltaTime;
	}
}