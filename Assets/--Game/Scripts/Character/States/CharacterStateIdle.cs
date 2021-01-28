using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateIdle : CharacterState
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
		characterRigidbody.UpdateCollision(1, -10);
	}

	public override void EndState(CharacterBase character)
	{

	}
}