using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateWallRun : CharacterState
{

	[SerializeField]
	CharacterState idleState;

	[SerializeField]
	CharacterRigidbody characterRigidbody;
	[SerializeField]
	CharacterMovement movement;

	[SerializeField]
	float stickRunThreshold = 0.7f;
	[SerializeField]
	float deccelerationRate = 0.7f;

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
		float axisX = Input.GetAxis("Horizontal");
		if (Mathf.Abs(axisX) > stickRunThreshold)
		{
			if(movement.SpeedX > 0)
			{
				characterRigidbody.UpdateCollision(movement.SpeedX * Mathf.Sign(axisX), movement.SpeedX);
				movement.SpeedX -= deccelerationRate * Time.deltaTime;
			}
			else
			{
				// On tombe
				movement.SpeedX -= deccelerationRate * Time.deltaTime;
				characterRigidbody.UpdateCollision(0, movement.SpeedX);
			}
		}
		else
		{
			characterRigidbody.UpdateCollision(0, 0);
			movement.SpeedX = 0;
			character.SetState(idleState);
		}
	}

	public override void EndState(CharacterBase character)
	{

	}
}