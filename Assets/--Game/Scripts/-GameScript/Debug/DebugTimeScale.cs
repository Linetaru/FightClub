using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DebugTimeScale : MonoBehaviour
{
	[OnValueChanged("SetTimeScale")]
	[SerializeField]
	float timeScale = 1f;

	private void SetTimeScale()
	{
		Time.timeScale = timeScale;
	}
}
