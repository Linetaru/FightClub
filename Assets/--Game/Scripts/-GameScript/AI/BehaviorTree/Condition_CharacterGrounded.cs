using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Condition_CharacterGrounded : Conditional
{
	public SharedCharacterBase character;

	public bool CheckAerial = false;
	public override TaskStatus OnUpdate()
	{
		if(CheckAerial)
			return !character.Value.Rigidbody.IsGrounded ? TaskStatus.Success: TaskStatus.Failure;
		else
			return character.Value.Rigidbody.IsGrounded ? TaskStatus.Success : TaskStatus.Failure;
	}


}
