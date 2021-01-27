using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public virtual void StartState(CharacterBase character)
	{

	}

	public virtual void UpdateState(CharacterBase character)
	{

	}

	public virtual void EndState(CharacterBase character)
	{

	}
}
