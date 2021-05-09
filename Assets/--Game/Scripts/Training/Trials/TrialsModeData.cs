using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public enum TrialsSpawnPoint
{
	BasePlayer,
	BaseEnemy,
	ClosePlayer,
	CloseEnemy,
	RangePlayer,
	RangeEnemy
}

[CreateAssetMenu(fileName = "TrialsData_", menuName = "Data/Training/TrialsData", order = 1)]
public class TrialsModeData : SerializedScriptableObject
{
	[SerializeField]
	string trialsName;
	public string TrialsName
	{
		get { return trialsName; }
	}


	[Title("Setup")]
	[SerializeField]
	string stageName;
	public string StageName
	{
		get { return stageName; }
	}

	[HorizontalGroup("Player")]
	[SerializeField]
	CharacterData player;
	public CharacterData Player
	{
		get { return player; }
	}

	[HorizontalGroup("Player", Width = 100)]
	[SerializeField]
	[HideLabel]
	TrialsSpawnPoint spawnPlayer;
	public TrialsSpawnPoint SpawnPlayer
	{
		get { return spawnPlayer; }
	}


	[HorizontalGroup("Enemy")]
	[SerializeField]
	CharacterData dummy;
	public CharacterData Dummy
	{
		get { return dummy; }
	}

	[HorizontalGroup("Enemy", Width = 100)]
	[SerializeField]
	[HideLabel]
	TrialsSpawnPoint spawnEnemy;
	public TrialsSpawnPoint SpawnEnemy
	{
		get { return spawnEnemy; }
	}

	[SerializeField]
	AIBehavior dummyBehavior;
	public AIBehavior DummyBehavior
	{
		get { return dummyBehavior; }
	}

	[Space]
	[Title("Texts")]
	[SerializeField]
	[HideLabel]
	private string osef;

	[TabGroup("Texts", "TextStart")]
	[SerializeField]
	[ListDrawerSettings(Expanded = true)]
	[TextArea(1,1)]
	List<string> textboxStart;
	public List<string> TextboxStart
	{
		get { return textboxStart; }
	}

	[TabGroup("Texts", "TextEnd")]
	[SerializeField]
	[ListDrawerSettings(Expanded = true)]
	[TextArea(1, 1)]
	List<string> textboxEnd;
	public List<string> TextboxEnd
	{
		get { return textboxEnd; }
	}


	[Space]
	[Title("Trials")]
	[SerializeField]
	int enemyPercentage = 0;
	public int EnemyPercentage
	{
		get { return enemyPercentage; }
	}
	[SerializeField]
	int numberToSuccess = 1;
	public int NumberToSuccess
	{
		get { return numberToSuccess; }
	}


	[OdinSerialize]
	[SerializeField]
	private List<MissionInputCondition> missions = new List<MissionInputCondition>();
	public List<MissionInputCondition> Missions
	{
		get { return missions; }
	}


	[SerializeField]
	List<string> comboNotes;
	public List<string> ComboNotes
	{
		get { return comboNotes; }
	}

	[Space]
	[Title("Fail Conditions")]

	[SerializeField]
	private List<MissionInputCondition> failConditions = new List<MissionInputCondition>();
	public List<MissionInputCondition> FailConditions
	{
		get { return failConditions; }
	}


	[Space]
	[Title("TextEnd")]


	[Space]
	[Space]
	[SerializeField]
	TrialsModeData nextTrial;
	public TrialsModeData NextTrial
	{
		get { return nextTrial; }
	}


}
