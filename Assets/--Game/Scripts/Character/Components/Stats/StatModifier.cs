using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum StatOperation
{
	Flat,
	Multiplier
}


[System.Serializable]
public class StatModifier
{
	[HorizontalGroup]
	[HideLabel]
	[SerializeField]
	private MainStat stat;
	public MainStat Stat
	{
		get { return stat; }
	}

	[HorizontalGroup]
	[HideLabel]
	[SerializeField]
	private StatOperation operation;
	public StatOperation Operation
	{
		get { return operation; }
	}

	[HorizontalGroup]
	[HideLabel]
	[SerializeField]
	private float value;
	public float Value
	{
		get { return value; }
	}


	public void ApplyModifier(IStats statManager)
	{
		// Le get stat est peut etre en trop, autant faire ça en une fois avec le apply stat modif peut etre
		Stats statCharacter = statManager.GetStat(stat);

		if (operation == StatOperation.Flat) 
		{
			statCharacter.IncrementFlatBonusStat(value);
		}
		else if (operation == StatOperation.Multiplier)
		{
			statCharacter.IncrementMultiplierBonusStat(value);
		}
		statManager.ApplyStatModifs(stat);
	}

	public void RevertModifier(IStats statManager)
	{
		// Le get stat est peut etre en trop, autant faire ça en une fois avec le apply stat modif peut etre
		Stats statCharacter = statManager.GetStat(stat);

		if (operation == StatOperation.Flat)
		{
			statCharacter.IncrementFlatBonusStat(-value);
		}
		else if (operation == StatOperation.Multiplier)
		{
			statCharacter.IncrementMultiplierBonusStat(-value);
		}
		statManager.ApplyStatModifs(stat);
	}

}
