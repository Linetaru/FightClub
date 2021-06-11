﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
	private string statusID;

	public string StatusID
	{
		get { return statusID; }
	}


	private List<StatusUpdate> statusUpdates;
	public List<StatusUpdate> StatusUpdates
	{
		get { return statusUpdates; }
	}

	private List<StatusEffect> statusEffects;
	public List<StatusEffect> StatusEffects
	{
		get { return statusEffects; }
	}


	public Status(string statusName, StatusData data)
	{
		statusID = statusName;

		statusUpdates = new List<StatusUpdate>(data.StatusUpdates.Length);
		for (int i = 0; i < data.StatusUpdates.Length; i++)
		{
			statusUpdates.Add(data.StatusUpdates[i].Copy());
		}


		statusEffects = new List<StatusEffect>(data.StatusEffects.Length);
		for (int i = 0; i < data.StatusEffects.Length; i++)
		{
			statusEffects.Add(data.StatusEffects[i].Copy());
		}
	}


	/// <summary>
	/// Update le status
	/// </summary>
	public void ApplyStatus(CharacterBase character)
	{
		for (int i = 0; i < statusEffects.Count; i++)
		{
			statusEffects[i].ApplyEffect(character);
		}
	}



	/// <summary>
	/// Update le status
	/// Renvois faux si le status doit se terminer
	/// </summary>
	/// <param name="character"></param>
	/// <returns></returns>
	public bool UpdateStatus(CharacterBase character)
	{
		for (int i = 0; i < statusUpdates.Count; i++)
		{
			if (statusUpdates[i].CanRemoveStatus(character) == true)
				return false;
		}

		for (int i = 0; i < statusEffects.Count; i++)
		{
			statusEffects[i].UpdateEffect(character);
		}
		return true;
	}


	/// <summary>
	/// Update le status
	/// </summary>
	public void RemoveStatus(CharacterBase character)
	{
		for (int i = 0; i < statusEffects.Count; i++)
		{
			statusEffects[i].RemoveEffect(character);
		}
	}


}