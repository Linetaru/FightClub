using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIC_Pathfind : MonoBehaviour
{

	[SerializeField]
	Pathfinding pathfinding;


	CharacterBase character;
	InputController inputController;
	Input_Info inputs;

	NavmeshNode destination;
	PathMovement currentMovement;

	Vector2 nodeDirection;
	bool calculatePath = true;
	bool inTheAir = false;
	float t = 0f;


	public void InitializeComponent(CharacterBase c, InputController input, Input_Info inputInfo)
	{
		character = c;
		inputController = input;
		inputs = inputInfo;
	}


	public void CalculatePath(Transform target)
	{
		pathfinding.CalculatePath(this.transform.position, target.transform.position);
		if (pathfinding.finalPath.Count != 0)
		{
			inTheAir = false;
			destination = pathfinding.finalPath[0].Node;
			currentMovement = pathfinding.finalPath[0].PreviousMovement;
			nodeDirection = destination.transform.position - this.transform.position;
			calculatePath = false;


			if (pathfinding.Position == pathfinding.Destination)
			{
				Debug.Log("On attend");
				currentMovement = PathMovement.Wait;
			}
			else if (destination == pathfinding.Position)
			{
				Debug.Log("On tente un double saut");
				destination = pathfinding.Destination;
				nodeDirection = pathfinding.Destination.transform.position - pathfinding.Position.transform.position;
				currentMovement = PathMovement.DoubleJump;
			}

			UpdateMovement();
		}
	}


	public void UpdatePath(Transform target)
	{
		if (calculatePath == false)
		{
			nodeDirection = destination.transform.position - this.transform.position;
			UpdateMovement();
		}
		else
		{
			CalculatePath(target);
		}
	}



	private void UpdateMovement()
	{
		switch (currentMovement)
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
			case PathMovement.DoubleJump:
				DoubleJumpToPoint();
				break;
			case PathMovement.Wait:
				Wait();
				break;
		}
	}

	private void RunToPoint()
	{
		inputController.AddMovement(Mathf.Sign(nodeDirection.x), 0, ref inputs);

		if (Mathf.Abs(nodeDirection.x) < 0.2f)
		{
			calculatePath = true;
		}
	}

	private void JumpToPoint()
	{
		if (character.Rigidbody.IsGrounded == true && inTheAir == false)
		{
			inputController.AddInput(InputConst.Jump.name, ref inputs);
			inputController.AddHoldInput(InputConst.Jump.name, ref inputs, true);
		}

		if (character.Rigidbody.IsGrounded == false && inTheAir == false)
		{
			inputs.inputActions[0].timeValue = 0;
			inputController.AddHoldInput(InputConst.Jump.name, ref inputs, false);
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
			if (character.CurrentState is CharacterStateIdle)
				inputController.AddInput(InputConst.Jump.name, ref inputs);
		}
		else
			inputController.AddMovement(Mathf.Sign(nodeDirection.x), 0, ref inputs);



		if (character.Rigidbody.IsGrounded == true && inTheAir == true)
		{
			calculatePath = true;
		}
	}



	private void DoubleJumpToPoint()
	{
		if (character.Rigidbody.IsGrounded == true && inTheAir == false)
		{
			inputController.AddInput(InputConst.Jump.name, ref inputs);
			inputController.AddMovement((destination.transform.position - this.transform.position).x / character.Movement.SpeedMax, 0, ref inputs);
			inputController.AddHoldInput(InputConst.Jump.name, ref inputs, true);
		}

		if (character.Rigidbody.IsGrounded == false && inTheAir == false)
		{
			inputs.inputActions[0].timeValue = 0;
			inputController.AddHoldInput(InputConst.Jump.name, ref inputs, false);
			inTheAir = true;
		}

		if (character.Movement.SpeedY < -1f && inTheAir == true && character.Movement.CurrentNumberOfJump != 0)
		{
			inputController.AddInput(InputConst.Jump.name, ref inputs);
			inputController.AddMovement((destination.transform.position - this.transform.position).x / character.Movement.SpeedMax, 0, ref inputs);
		}

		else if (character.Movement.SpeedY < -2f && inTheAir == true && character.Movement.CurrentNumberOfJump == 0)
		{
			//inputController.AddInput(InputConst.Jump.name, ref inputs);
			destination = FindNearestNode(this.transform);
			inputController.AddMovement(Mathf.Sign((destination.transform.position - this.transform.position).x), 0, ref inputs);
		}

		if (character.Rigidbody.IsGrounded == true && inTheAir == true)
			calculatePath = true;
	}



	private void Wait()
	{
		inputController.AddMovement(0, 0, ref inputs);
		t += Time.deltaTime;
		if (t >= 1)
		{
			t = 0f;
			calculatePath = true;
		}
	}










	public NavmeshNode FindNearestNode(Transform pos)
	{
		return pathfinding.FindNearest(pos.position);
	}



}
