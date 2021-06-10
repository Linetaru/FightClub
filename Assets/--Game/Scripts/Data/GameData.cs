using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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


	[HorizontalGroup("GameMode")]
	[SerializeField]
	private GameMode[] gameModes = null;

	[HorizontalGroup("GameMode")]
	[SerializeField]
	private GameModeSettings[] gameSettings = null;



	private GameMode gameModePrefab = null;
	public GameMode GameModePrefab
	{
		get { return gameModePrefab; }
		set { gameModePrefab = value; }
	}

	private GameModeSettings gameSetting = null;
	public GameModeSettings GameSetting
	{
		get { return gameSetting; }
		set { gameSetting = value; }
	}


	[HideInInspector]
	public bool slamMode = false;


	public void SetGameSettings()
	{
		SetGameSettings(gameMode);
	}
	public void SetGameSettings(GameModeStateEnum gameMode)
	{
		gameSetting = gameSettings[(int)gameMode];
	}



	public GameMode CreateGameMode()
	{
		return CreateGameMode(gameMode);
	}
	public GameMode CreateGameMode(GameModeStateEnum gameMode)
	{
		return gameModes[(int)gameMode];
	}

	public int GetModeScoreGoal(GameModeStateEnum gameMode)
    {
		return gameSettings[(int)gameMode].ScoreGoal;
	}
	public void SetModeScoreGoal(GameModeStateEnum gameMode, int scoreGoal)
	{
		gameSettings[(int)gameMode].ScoreGoal = scoreGoal;
	}

}