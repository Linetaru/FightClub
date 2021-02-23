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
	public void Jump()
	{
		characterMovement.Jump();
	}

	// Character Action
	public void ActionActive()
	{
		characterAction.ActionActive();
	}

	public void ActionUnactive()
	{
		characterAction.ActionUnactive();
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
