using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	[SerializeField]
	CharacterBase characterBase;


	[SerializeField]
	Animator animator;
	[SerializeField]
	CharacterMovement movement;

	[SerializeField]
	float speedDelta = 30;

	// Start is called before the first frame update
	void Start()
	{
		characterBase.OnStateChanged += CheckState;
	}

	public void CheckState(CharacterState oldState, CharacterState newState)
	{
		if (newState is CharacterStateIdle)
			animator.SetTrigger("Idle");
		if (newState is CharacterStateAerial)
			animator.SetTrigger("Fall");
	}

	// Update is called once per frame
	void Update()
	{
		float speedT = movement.SpeedX / (movement.MaxSpeed - speedDelta);
		animator.SetFloat("Speed", Mathf.Clamp(speedT, 0, 1));

		if (movement.Direction == 1)
			animator.transform.localScale = Vector3.one;
		else if (movement.Direction == -1)
			animator.transform.localScale = new Vector3(1,1,-1);
	}

	void OnDestroy()
	{
		characterBase.OnStateChanged -= CheckState;
	}
}
