using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Sert de proxy pour les events d'animations
public class CharacterAnimatorEvent : MonoBehaviour
{
	[SerializeField]
	CharacterMovement characterMovement;

	[SerializeField]
	CharacterAction characterAction;

	public void MoveForward(float multiplier)
	{
		characterMovement.MoveForward(multiplier);
	}
	public void Jump(float multiplier)
	{
		characterMovement.Jump(multiplier);
	}

	// Character Action
	public void ActionActive(int subAction = 0)
	{
		characterAction.ActionActive(subAction);
	}

	public void ActionUnactive(int subAction = 0)
	{
		characterAction.ActionUnactive(subAction);
	}

	public void MoveCancelable()
	{
		characterAction.MoveCancelable();
	}

	public void EndAction()
	{
		characterAction.EndAction();
	}
}
