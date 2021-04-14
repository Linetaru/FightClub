using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParticle : MonoBehaviour
{
	public GameObject jumpParticle;
	public ParticleSystem startJumpParticle;
	//public GameObject wParticules;
	//public GameObject xParticules;
	//public GameObject cParticules;

	public void UseParticle(string particleName)
    {
		switch(particleName)
        {
			case "jump":
				jumpParticle.transform.GetComponentInChildren<ParticleSystem>().Play();
				break;
        }
	}
}