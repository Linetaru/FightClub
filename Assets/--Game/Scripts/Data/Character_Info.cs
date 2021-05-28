using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Character_Info
{
	[HorizontalGroup]
	[SerializeField]
	private int controllerID;
	public int ControllerID
	{
		get { return controllerID; }
		set { controllerID = value; }
	}

	[HorizontalGroup]
	[HideLabel]
	[SerializeField]
	private InputMappingData inputMapping;
	public InputMappingData InputMapping
	{
		get { return inputMapping; }
		set { inputMapping = value; }
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

	public Character_Info()
	{
		controllerID = 0;
		team = TeamEnum.No_Team;
		characterColorID = 0;
		characterData = null;
	}
}