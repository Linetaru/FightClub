using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTrigger : MonoBehaviour
{
	[SerializeField]
	UnityEvent unityEvent;

	[SerializeField]
	bool callMultipleTime = false;

	private void OnTriggerEnter(Collider other)
	{
		other.GetComponent<CharacterBase>();
		if(other != null)
		{
			unityEvent.Invoke();
			if (callMultipleTime == false)
				Destroy(this.gameObject);
		}
	}

}
