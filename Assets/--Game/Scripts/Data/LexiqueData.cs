using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "LexiqueData", menuName = "Data/LexiqueData", order = 1)]
public class LexiqueData : ScriptableObject
{

	[SerializeField]
	string entryTitle = "";
	public string EntryTitle
	{
		get { return entryTitle; }
	}

	[SerializeField]
	[TextArea(4, 10)]
	private string entryText = "";
	public string EntryText
	{
		get { return entryText; }
	}

	[SerializeField]
	private Sprite entrySprite = null;
	public Sprite EntrySprite
	{
		get { return entrySprite; }
	}

	[Space]
	[SerializeField]
	private bool music = false;

	[SerializeField]
	[ShowIf("music")]
	private AK.Wwise.Event eventSound = null;
	public AK.Wwise.Event EventSound
	{
		get { return eventSound; }
	}

}
