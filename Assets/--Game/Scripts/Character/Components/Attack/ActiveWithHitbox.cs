using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWithHitbox : MonoBehaviour
{
	[SerializeField]
	Collider collider;
	[SerializeField]
	ParticleSystem particle;

	bool stop = false;
	void Start()
	{
		particle.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (collider.enabled == true && stop == false)
		{
			particle.gameObject.SetActive(true);
			stop = true;
		}

	}
}
