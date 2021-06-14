using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "Settings", menuName = "Data/GameModeSettings/Settings", order = 1)]
public class GameModeSettings : ScriptableObject
{
    [SerializeField]
    SODatabase_Stages stagesAvailable;
	public SODatabase_Stages StagesAvailable
	{
		get { return stagesAvailable; }
	}


	[SerializeField]
	bool canSelectCPU = false;
	public bool CanSelectCPU
	{
		get { return canSelectCPU; }
	}


	[SerializeField]
	bool skipIntro = false;
	public bool SkipIntro
	{
		get { return skipIntro; }
		set { skipIntro = value; }
	}

	[SerializeField]
	bool hasScoreGoal = false;
	public bool HasScoreGoal
	{
		get { return hasScoreGoal; }
		set { hasScoreGoal = value; }
	}

	[SerializeField]
	[ShowIf("hasScoreGoal")]
	int scoreGoal = 5;
	public int ScoreGoal
	{
		get { return scoreGoal; }
		set { scoreGoal = value; }
	}
}
