using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Condition_CharacterSpeed : Conditional
{
	public SharedCharacterBase character;

	public float SpeedY = -5f;

	public override TaskStatus OnUpdate()
	{
		return (character.Value.Movement.SpeedY < SpeedY) ? TaskStatus.Success : TaskStatus.Failure;
	}

}
