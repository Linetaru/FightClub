using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnAwake : MonoBehaviour
{
	[SerializeField]
	UnityEvent unityEvent;
	private void OnEnable()
	{
		unityEvent.Invoke();
	}
}
