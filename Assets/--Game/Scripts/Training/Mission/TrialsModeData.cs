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

}
