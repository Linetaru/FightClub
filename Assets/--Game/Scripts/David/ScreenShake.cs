using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Feedbacks
{
	public class ScreenShake : MonoBehaviour
	{
		// Transform of the camera to shake. Grabs the gameObject's transform
		// if null.
		public Transform camTransform;

		// How long the object should shake for.
		public float currentshakeDuration = 0f;

		// Amplitude of the shake. A larger value shakes the camera harder.
		public float shakeAmount_d = 0.7f;
		public float decreaseFactor_d = 0.1f;

		Vector3 originalPos;

		void Awake()
		{
			if (camTransform == null)
			{
				camTransform = GetComponent(typeof(Transform)) as Transform;
			}
		}

		void OnEnable()
		{
			originalPos = camTransform.localPosition;
		}

		public void StartScreenShake(float shakeAmount, float shakeDuration)
		{
			shakeAmount_d = shakeAmount;
			currentshakeDuration = shakeDuration / 10;
		}

		void Update()
		{
			if (currentshakeDuration > 0)
			{
				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount_d;

				currentshakeDuration -= Time.deltaTime * decreaseFactor_d;
			}
			else
			{
				currentshakeDuration = 0f;
				camTransform.localPosition = originalPos;
			}
		}
	}
}
