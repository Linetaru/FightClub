using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Condition_CharacterDistance : Conditional
{
	public SharedCharacterBase character;

	public float Distance = 2f;

	public override TaskStatus OnUpdate()
	{
		return (Vector2.Distance(character.Value.transform.position, this.transform.position) < Distance) ? TaskStatus.Success : TaskStatus.Failure;
	}


}
