using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState : MonoBehaviour
{

	public virtual void StartState(CharacterBase character, CharacterState oldState)
	{

	}

	/// <summary>
	/// Update avant le check de collision
	/// </summary>
	/// <param name="character"></param>
	public virtual void UpdateState(CharacterBase character)
	{

	}

	/// <summary>
	/// Update après le check de collision
	/// </summary>
	/// <param name="character"></param>
	public virtual void LateUpdateState(CharacterBase character)
	{

	}

	public virtual void EndState(CharacterBase character, CharacterState newState)
	{

	}
}
