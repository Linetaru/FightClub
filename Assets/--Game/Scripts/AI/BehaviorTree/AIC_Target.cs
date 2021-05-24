using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AIC_Target : MonoBehaviour
{
	[SerializeField]
	List<CharacterBase> targets = new List<CharacterBase>();

	public CharacterBase GetRandomTarget()
	{
		if (targets == null)
			return null;
		if (targets.Count == 0)
			return null;
		return targets[Random.Range(0, targets.Count - 1)];
	}

	private void OnTriggerEnter(Collider other)
	{
		CharacterBase c = other.GetComponent<CharacterBase>();
		if (c != null)
		{
			if (!targets.Contains(c))
				targets.Add(c);
		}
	}

}
