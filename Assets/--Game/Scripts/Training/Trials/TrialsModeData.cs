using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "TrialsData_", menuName = "Data/Training/TrialsData", order = 1)]
public class TrialsModeData : SerializedScriptableObject
{
	[SerializeField]
	CharacterData player;
	public CharacterData Player
	{
		get { return player; }
	}

	[SerializeField]
	CharacterData dummy;
	public CharacterData Dummy
	{
		get { return dummy; }
	}

	[SerializeField]
	AIBehavior dummyBehavior;
	public AIBehavior DummyBehavior
	{
		get { return dummyBehavior; }
	}

	[Space]
	[Title("TextStart")]
	[SerializeField]
	List<string> textboxStart;
	public List<string> TextboxStart
	{
		get { return textboxStart; }
	}




	[Space]
	[Title("Trials")]
	[SerializeField]
	int enemyPercentage = 0;
	public int EnemyPercentage
	{
		get { return enemyPercentage; }
	}



	[OdinSerialize]
	[SerializeField]
	[ListDrawerSettings(Expanded = true)]
	private List<MissionInputCondition> missions = new List<MissionInputCondition>();
	public List<MissionInputCondition> Missions
	{
		get { return missions; }
	}


	[SerializeField]
	[ListDrawerSettings(Expanded = true)]
	List<string> comboNotes;
	public List<string> ComboNotes
	{
		get { return comboNotes; }
	}

	[Space]
	[Title("Fail Conditions")]
	[SerializeField]
	private int numberOfTry = -1;


	[SerializeField]
	[ListDrawerSettings(Expanded = true)]
	private List<MissionInputCondition> failConditions = new List<MissionInputCondition>();
	public List<MissionInputCondition> FailConditions
	{
		get { return failConditions; }
	}


	[Space]
	[Title("TextEnd")]
	[SerializeField]
	List<string> textboxEnd;
	public List<string> TextboxEnd
	{
		get { return textboxEnd; }
	}

	[Space]
	[Space]
	[SerializeField]
	TrialsModeData nextTrial;
	public TrialsModeData NextTrial
	{
		get { return nextTrial; }
	}
}
