using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character_Info
{
	[SerializeField]
	private int controllerID;
	public int ControllerID
	{
		get { return controllerID; }
		set { controllerID = value; }
	}

	[SerializeField]
	private TeamEnum team;
	public TeamEnum Team
	{
		get { return team; }
		set { team = value; }
	}

	[SerializeField]
	private int characterColorID;
	public int CharacterColorID
	{
		get { return characterColorID; }
		set { characterColorID = value; }
	}

	[Expandable]
	[SerializeField]
	private CharacterData characterData;
	public CharacterData CharacterData
	{
		get { return characterData; }
		set { characterData = value; }
	}
}