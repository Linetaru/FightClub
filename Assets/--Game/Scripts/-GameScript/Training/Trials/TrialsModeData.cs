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
	string trialsName = "";
	public string TrialsName
	{
		get { return trialsName; }
	}

	[SerializeField]
	[TextArea]
	string trialsDescription = "";
	public string TrialsDescription
	{
		get { return trialsDescription; }
	}


	[Title("Setup")]
	[SerializeField]
	[Scene]
	string scene = "";
	public string StageName
	{
		get { return scene; }
	}

	[HorizontalGroup("Player")]
	[SerializeField]
	CharacterData player = null;
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
	CharacterData dummy = null;
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
	AIBehavior dummyBehavior = null;
	public AIBehavior DummyBehavior
	{
		get { return dummyBehavior; }
	}

	[Space]
	[Title("Texts")]
	[SerializeField]
	[HideLabel]
	private string osef = "";

	[TabGroup("Texts", "TextStart")]
	[SerializeField]
	[ListDrawerSettings(Expanded = true)]
	[TextArea(1, 1)]
	List<string> textboxStart = new List<string>();
	public List<string> TextboxStart
	{
		get { return textboxStart; }
	}

	[TabGroup("Texts", "TextEnd")]
	[SerializeField]
	[ListDrawerSettings(Expanded = true)]
	[TextArea(1, 1)]
	List<string> textboxEnd = new List<string>();
	public List<string> TextboxEnd
	{
		get { return textboxEnd; }
	}

	[Space]
	[Space]
	[Space]
	[Title("Trials")]
	[SerializeField]
	int enemyPercentage = 0;
	public int EnemyPercentage
	{
		get { return enemyPercentage; }
	}
	[SerializeField]
	int gaugeNumber = 4;
	public int GaugeNumber
	{
		get { return gaugeNumber; }
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

	[HorizontalGroup("Notes")]
	[SerializeField]
	List<string> comboNotes = new List<string>();
	public List<string> ComboNotes
	{
		get { return comboNotes; }
	}

	[HorizontalGroup("Notes")]
	[SerializeField]
	List<TrialsButton> trialsButtonsNote = new List<TrialsButton>();
	public List<TrialsButton> TrialsButtonsNote
	{
		get { return trialsButtonsNote; }
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
	[Title("Reward")]

	[SerializeField]
	int moneyReward = 0;
	public int MoneyReward
	{
		get { return moneyReward; }
	}


	[Space]
	[Space]
	[SerializeField]
	TrialsModeData nextTrial = null;
	public TrialsModeData NextTrial
	{
		get { return nextTrial; }
	}


}
