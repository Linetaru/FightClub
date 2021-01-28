using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DebugStat : MonoBehaviour
{
	[SerializeField]
	Stats speed;

	//public float speedValue;


	[Title("Debug")]
	[SerializeField]
	float value;
	[SerializeField]
	bool addition;

	[Button("Add")]
	public void AddStat()
	{
		if(addition == true)
			speed.IncrementBonusStat();
		else
			speed.IncrementBonusStat();
	}

	[Button("Remove")]
	public void RemoveStat()
	{
		if (addition == true)
			speed.RemoveBonusStat();
		else
			speed.RemoveBonusStat();
	}


	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
