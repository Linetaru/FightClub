using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VictoryCondition{
	Health,
	Timer,
}

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
public class GameData : ScriptableObject
{
	[SerializeField]
	private List<Character_Info> characterInfos = new List<Character_Info>();
	public List<Character_Info> CharacterInfos
	{
		get { return characterInfos; }
		set { characterInfos = value; }
	}

	[SerializeField] [HideInInspector]
	private VictoryCondition victoryCondition = VictoryCondition.Health;
	public VictoryCondition VictoryCondition
	{
		get { return victoryCondition; }
		set { victoryCondition = value; }
	}

	[SerializeField] [HideInInspector]
	private int numberOfLifes = 3;
	public int NumberOfLifes
	{
		get { return numberOfLifes; }
		set { numberOfLifes = value; }
	}

	[SerializeField] [HideInInspector]
	private float timeOfRoundInSecond = 30f;
	public float TimeOfRoundInSecond
	{
		get { return timeOfRoundInSecond; }
		set { timeOfRoundInSecond = value; }
	}

	[SerializeField] [HideInInspector]
	private GameModeStateEnum gameMode = GameModeStateEnum.Classic_Mode;
	public GameModeStateEnum GameMode
	{
		get { return gameMode; }
		set { gameMode = value; }
	}

	public bool slamMode = false;
}