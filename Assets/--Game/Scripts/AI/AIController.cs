using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AIController : MonoBehaviour
{
	[SerializeField]
	AIBehavior defaultBehavior;


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

	public AIBehavior CreateDefaultBehavior(CharacterBase character, InputController input)
	{
		AIBehavior aIBehavior = Instantiate(defaultBehavior, character.transform);
		aIBehavior.SetCharacter(character, input);
		aIBehavior.StartBehavior();
		AIBehaviors.Add(aIBehavior);
		return aIBehavior;
	}

	public void RemoveBehavior(CharacterBase character)
	{
		for (int i = 0; i < AIBehaviors.Count; i++)
		{
			if (AIBehaviors[i].Character == character)
			{
				AIBehavior refCopy = AIBehaviors[i];
				AIBehaviors[i].StopBehavior();
				AIBehaviors.RemoveAt(i);
				Destroy(refCopy.gameObject);
				return;
			}

		}
	}
}
