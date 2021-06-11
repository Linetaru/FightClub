using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public enum CharacterStateID
{
	Action,
	Hit,
	Dead,
	Parry,
	ParrySuccess,
	ParryRepel,
	Wall
}

public class Condition_CharacterState : Conditional
{
	public SharedCharacterBase character;

	public CharacterStateID stateID;

	public override TaskStatus OnUpdate()
	{
		switch(stateID)
		{
			case CharacterStateID.Action:
				return (character.Value.CurrentState is CharacterStateActing) ? TaskStatus.Success : TaskStatus.Failure;
			case CharacterStateID.Hit:
				return (character.Value.CurrentState is CharacterStateKnockback) ? TaskStatus.Success : TaskStatus.Failure;
			case CharacterStateID.Dead:
				return (character.Value.CurrentState is CharacterStateDeath) ? TaskStatus.Success : TaskStatus.Failure;
			case CharacterStateID.Parry:
				return (character.Value.CurrentState is CharacterStateParry) ? TaskStatus.Success : TaskStatus.Failure;
			case CharacterStateID.ParrySuccess:
				return (character.Value.CurrentState is CharacterStateParrySuccess) ? TaskStatus.Success : TaskStatus.Failure;
			case CharacterStateID.ParryRepel:
				return (character.Value.CurrentState is CharacterStateParryBlow) ? TaskStatus.Success : TaskStatus.Failure;
			case CharacterStateID.Wall:
				return (character.Value.CurrentState is CharacterStateWallRun) ? TaskStatus.Success : TaskStatus.Failure;
		}
		return TaskStatus.Failure;
	}
}
