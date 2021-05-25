using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AIC_Target : MonoBehaviour
{
	[SerializeField]
	List<CharacterBase> targets = new List<CharacterBase>();

	CharacterBase self;

	public void InitializeComponent(CharacterBase c, InputController input, Input_Info inputInfo)
	{
		self = c;
		targets.Remove(self);

	}

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
			if (c == self)
				return;
			if (!targets.Contains(c))
				targets.Add(c);
		}
	}

}
