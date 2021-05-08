using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviorTest : AIBehavior
{
	[SerializeField]
	Pathfinding pathfinding;

	[SerializeField]
	Transform target;

	NavmeshNode destination;
	PathMovement currentMovement;

	Vector2 nodeDirection;
	bool calculatePath = true;
	bool inTheAir = false;


	// Update is called once per frame
	void Update()
	{
		if (isActive == false)
			return;

		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActions);
		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActionsUP);

		if (calculatePath == true)
		{
			pathfinding.CalculatePath(this.transform.position, target.transform.position);
			if (pathfinding.finalPath.Count != 0)
			{
				inTheAir = false;
				destination = pathfinding.finalPath[0].Node;
				currentMovement = pathfinding.finalPath[0].PreviousMovement;
				nodeDirection = destination.transform.position - this.transform.position;
				calculatePath = false;
				UpdateMovement();
			}
		}
		else
		{
			nodeDirection = destination.transform.position - this.transform.position;
			UpdateMovement();
		}
		character.UpdateControl(0, inputs);
	}


	private void UpdateMovement()
	{
		switch(currentMovement)
		{
			case PathMovement.Run:
				RunToPoint();
				break;
			case PathMovement.Fall:
				FallToPoint();
				break;
			case PathMovement.Jump:
				JumpToPoint();
				break;
		}
	}

	private void RunToPoint()
	{
		inputController.AddMovement(Mathf.Sign(nodeDirection.x), 0, ref inputs);

		if(Mathf.Abs(nodeDirection.x) < 0.2f)
		{
			calculatePath = true;
		}
	}

	private void JumpToPoint()
	{
		if (character.Rigidbody.IsGrounded == true && inTheAir == false) 
		{
			inputController.AddInput(InputConst.Jump.name, ref inputs);
		}

		if (character.Rigidbody.IsGrounded == false && inTheAir == false)
		{
			inTheAir = true;
		}

		if (Mathf.Abs(nodeDirection.x) >= 0.2f)
			inputController.AddMovement(Mathf.Sign(nodeDirection.x), 0, ref inputs);
		else
			inputController.AddMovement(0, 0, ref inputs);

		if (character.Rigidbody.IsGrounded == true && inTheAir == true)
			calculatePath = true;
	}

	private void FallToPoint()
	{
		if (character.Rigidbody.IsGrounded == true && inTheAir == false)
		{
			inputController.AddMovement(Mathf.Sign(nodeDirection.x), 0, ref inputs);
		}
		if (character.Rigidbody.IsGrounded == false && inTheAir == false)
		{
			inTheAir = true;
			inputController.AddMovement(0, 0, ref inputs);
		}

		if (Mathf.Abs(nodeDirection.x) <= 0.2f && character.Rigidbody.IsGrounded == true)
		{
			inputController.AddMovement(0, -1, ref inputs);
			if(character.CurrentState is CharacterStateIdle)
				inputController.AddInput(InputConst.Jump.name, ref inputs);
		}
		else
			inputController.AddMovement(Mathf.Sign(nodeDirection.x), 0, ref inputs);



		if (character.Rigidbody.IsGrounded == true && inTheAir == true)
		{
			calculatePath = true;
		}
	}
}
