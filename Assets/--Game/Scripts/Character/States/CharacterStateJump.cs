using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateJump : CharacterState
{

	[SerializeField]
	CharacterRigidbody characterRigidbody;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public override void StartState(CharacterBase character)
	{

	}

	public override void UpdateState(CharacterBase character)
	{
		characterRigidbody.UpdateCollision(0, -10f);
	}

	public override void EndState(CharacterBase character)
	{

	}
}