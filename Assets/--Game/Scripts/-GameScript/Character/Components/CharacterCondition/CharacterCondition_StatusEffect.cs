using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCondition_StatusEffect : CharacterCondition
{
	[SerializeField]
	string statusID;

	[SerializeField]
	bool reverse = false;


	public override bool CheckCondition(CharacterBase character)
	{
		if(reverse == true)
			return !character.Status.ContainsStatus(statusID);
		return character.Status.ContainsStatus(statusID);
	}
}
