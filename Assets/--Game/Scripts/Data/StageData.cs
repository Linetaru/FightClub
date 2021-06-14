using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "StageData", menuName = "Data/StageData", order = 1)]
public class StageData : ScriptableObject
{

	[HideLabel]
	[HorizontalGroup("Stage", Width = 96)]
	[PreviewField(ObjectFieldAlignment.Left, Height = 96)]
	[SerializeField]
	Sprite stageThumbnail;
	public Sprite StageThumbnail
	{
		get { return stageThumbnail; }
	}

	[HorizontalGroup("Stage", PaddingLeft = 10)]
	[VerticalGroup("Stage/Right")]
	[SerializeField]
	string stageName;
	public string StageName
	{
		get { return stageName; }
	}

	[VerticalGroup("Stage/Right")]
	[SerializeField]
	[TextArea(4, 5)]
	private string stageDescription;
	public string StageDescription
	{
		get { return stageDescription; }
	}


	[SerializeField]
	[Scene]
	private string sceneName;
	public string SceneName
	{
		get { return sceneName; }
	}


}
