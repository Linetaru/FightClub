using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AIController : MonoBehaviour
{
	public List<AIBehavior> AIBehaviors;

	[Button]
	public void StartBehaviors()
	{
		for (int i = 0; i < AIBehaviors.Count; i++)
		{
			AIBehaviors[i].StartBehavior();
		}
	}

	public void StopBehaviors()
	{
		for (int i = 0; i < AIBehaviors.Count; i++)
		{
			AIBehaviors[i].StopBehavior();
		}
	}
}
