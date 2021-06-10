using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShoulderShake : MonoBehaviour
{
	[SerializeField]
	float power = 0.1f;
	[SerializeField]
	float coef = 0.5f;
	[SerializeField]
	Vector2 timeShake = Vector2.one;

	float t = 0f;

	float tMax = 0f;
	Vector3 destination;


	// Update is called once per frame
	void Update()
	{
		t += Time.deltaTime;
		this.transform.position = Vector3.Lerp(this.transform.position, destination, coef);

		if (t > tMax)
		{
			t = 0;
			tMax = Random.Range(timeShake.x, timeShake.y);
			destination = new Vector3(Random.Range(-power, power), Random.Range(-power, power), Random.Range(-power, power));
			//speed = (destination - this.transform.position) / tMax;
		}
	}
}
