using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum Collectible_type
{
	None,
	BasicBoost,
	MegaBoost,
}

[RequireComponent(typeof(SphereCollider),(typeof(Rigidbody)))]
public abstract class CollectibleObject : MonoBehaviour
{
	[Title("Type of Collectible")]
	public Collectible_type collectibleType;

    private void Reset()
    {
		GetComponent<SphereCollider>().isTrigger = true;
		GetComponent<Rigidbody>().isKinematic = true;
    }

	public virtual void OnTriggerEnter(Collider other)
	{
		for (int i = 1; i < 5; i++)
		{
			if (other.tag == "Player" + i)
			{
				other.GetComponent<CharacterBase>().PowerGauge.AddBoost(collectibleType);
				Destroy(this.gameObject);
			}
		}
	}
}