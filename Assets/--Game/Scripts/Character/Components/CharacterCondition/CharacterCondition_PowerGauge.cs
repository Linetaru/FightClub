using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCondition_PowerGauge : CharacterCondition
{

	[SerializeField]
	float requiredPowerGauge = 10;

	public override bool CheckCondition(CharacterBase character)
	{
		return (character.PowerGauge.CurrentPower >= requiredPowerGauge);
	}

}
