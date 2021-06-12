using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ne gère pas les collisions avec plusieurs objets
public struct CollisionRigidbody
{
	[SerializeField]
	public Transform Collision;

	[SerializeField]
	public bool[] Contacts;

	public CollisionRigidbody(int nbRaycast)
	{
		Collision = null;
		Contacts = new bool[nbRaycast];
		for (int i = 0; i < Contacts.Length; i++)
		{
			Contacts[i] = false;
		}
	}
}
