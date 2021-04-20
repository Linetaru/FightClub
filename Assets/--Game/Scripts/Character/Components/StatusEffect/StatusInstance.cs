using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Classe utilisé pour serializer des status avec un ID
[System.Serializable]
public class StatusInstance
{
	[SerializeField]
	string statusID;

	[SerializeField]
	StatusData statusData;

	public Status CreateStatus()
	{
		return new Status(statusID, statusData);
	}
}
