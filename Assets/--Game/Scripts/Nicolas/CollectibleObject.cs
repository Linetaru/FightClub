using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//Type of collectible for reference
public enum Collectible_type
{
	None,
	BasicBoost,
	MegaBoost,
}

//Entity boost with requireComponent to be able to call this components
[RequireComponent(typeof(SphereCollider),(typeof(Rigidbody)))]
public abstract class CollectibleObject : MonoBehaviour
{
	//access of boost type is it
	[Title("Type of Collectible")]
	public Collectible_type collectibleType;

	//Method to set automaticaly when script is apply on a object
    private void Reset()
    {
		GetComponent<SphereCollider>().isTrigger = true;
		GetComponent<Rigidbody>().isKinematic = true;
    }

	//Trigger to check which player take it and give boost at the player before destroy itself
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