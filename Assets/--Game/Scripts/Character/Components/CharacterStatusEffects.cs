using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusEffects : MonoBehaviour
{
	List<Status> status = new List<Status>();

	CharacterBase character;

	public void InitializeComponent(CharacterBase characterBase)
	{
		character = characterBase;
	}


	public bool ContainsStatus(string statusID)
	{
		for (int i = 0; i < status.Count; i++)
		{
			if (status[i].StatusID.Equals(statusID))
				return true;
		}
		return false;
	}

	public void AddStatus(Status newStatus)
	{
		if(ContainsStatus(newStatus.StatusID) == false) // On ne peut pas ajouter 2x le meme status (pour l'instant)
			status.Add(newStatus);
	}

	public void UpdateStatus()
	{
		for (int i = status.Count-1; i >= 0; i--)
		{
			if (status[i].UpdateStatus(character) == false)
				RemoveStatus(status[i]);
		}
	}

	public void RemoveStatus(Status newStatus)
	{
		status.Remove(newStatus);
	}
}
