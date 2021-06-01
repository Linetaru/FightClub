using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsMission", menuName = "Data/GameModeSettings/SettingsMission", order = 1)]
public class GameModeSettingsMission : GameModeSettings
{
	[SerializeField]
	private TrialsModeData trialsData;
	public TrialsModeData TrialsData
	{
		get { return trialsData; }
		set { trialsData = value; }
	}

	[SerializeField]
	private SODatabase_Mission trialsDatabase;
	public SODatabase_Mission TrialsDatabase
	{
		get { return trialsDatabase; }
		set { trialsDatabase = value; }
	}

	private int previousMenu;
	public int PreviousMenu
	{
		get { return previousMenu; }
		set { previousMenu = value; }
	}

}
