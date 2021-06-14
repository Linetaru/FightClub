using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Condition_Recovery : Conditional
{
	public SharedCharacterBase character;

	public float distanceRaycastDown = 15;
	public float speedFall = -5f;
	public LayerMask layerMask;

	public override TaskStatus OnUpdate()
	{
		if(character.Value.Movement.SpeedY < speedFall)
		{
			if(!Physics.Raycast(this.transform.position, Vector3.down, distanceRaycastDown, layerMask))
			{
				return TaskStatus.Success;
			}
		}
		return TaskStatus.Failure;
	}
}
