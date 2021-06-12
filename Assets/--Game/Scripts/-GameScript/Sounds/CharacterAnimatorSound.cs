using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AkGameObj))]
public class CharacterAnimatorSound : MonoBehaviour
{
	[SerializeField]
	AK.Wwise.Event defaultSound = null;

	public void PlaySound(string soundName)
	{
		AkSoundEngine.PostEvent(soundName, this.gameObject);
	}

	public void APlaySound()
	{
		AkSoundEngine.PostEvent(defaultSound.Id, this.gameObject);
	}
}
