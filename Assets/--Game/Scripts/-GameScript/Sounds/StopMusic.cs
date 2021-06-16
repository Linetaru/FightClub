using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StopMusic : MonoBehaviour
{
	[SerializeField]
	AK.Wwise.Event stopEvent = null;
	[SerializeField]
	[PropertyRange(0, 10)]
	float msTransition = 1;

	[SerializeField]
	AkCurveInterpolation curveFade = AkCurveInterpolation.AkCurveInterpolation_Linear;

	[Button]
	public void Stop()
	{
		stopEvent.ExecuteAction(this.gameObject, AkActionOnEventType.AkActionOnEventType_Stop, (int) (msTransition * 1000), curveFade);
	}

	private void OnDisable()
	{
		Stop();
	}

}
