using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWithHitbox : MonoBehaviour
{
	[SerializeField]
	Collider collider;
	[SerializeField]
	ParticleSystem particle;
	[SerializeField]
	GameObject a;

	bool stop = false;
	void Start()
	{
		if (particle != null)
			particle.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (collider.enabled == true && stop == false)
		{
			if(particle != null)
				particle.gameObject.SetActive(true);
			stop = true;
		}

		if (collider.enabled == true)
		{
			if(a!= null)
				a.gameObject.SetActive(true);
		}
		else if (collider.enabled == false)
		{
			if (a != null)
				a.gameObject.SetActive(false);
		}

	}
}
